using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkGameManager : NetworkBehaviour {

    // singleton
    static public NetworkGameManager sInstance ;
    static public List<GameObject> players = new List<GameObject>();
    public scoreboardCell scoreboard;
    public RectTransform playerListRect ;
    [SerializeField]
    int winningPoints = 30;

    List<scoreboardCell> scoreboardCells = new List<scoreboardCell>();


    public float respawnTime = 2f;

    bool mResult = false;

	
	void Awake () {
        //singleton
        sInstance = this ;
    }

    [ServerCallback]
    void Start() {
        StartCoroutine(delayUpdateScoreboard());
    }
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {
      
    }

    IEnumerator delayUpdateScoreboard()
    {
        yield return new WaitForSeconds(2);
        UpdateScoreboard();
    }



    [ServerCallback]
    void RpcUpdateScoreboard()
    {
        RpcInitScoreboard();
        // update scoreboard locally.
        for (int i = 0; i < NetworkGameManager.players.Count; i++)
        {
            NetworkPlayer p = NetworkGameManager.players[i].GetComponent<NetworkPlayer>();
            //scoreboardCells[i].RpcUpdateScore(p.playerColor,p.playerName,p.kill_count,p.death_count,p.point);
            RpcReceiveUpdateScore(i,p.playerColor, p.playerName, p.kill_count, p.death_count, p.point);
        }
    }

    [ClientRpc]
    void RpcReceiveUpdateScore( int index, Color playerColor, string playerName, int kill_count, int death_count, int point)
    {
        if ( index < scoreboardCells.Count )
        {
            scoreboardCells[index].UpdateScore(playerColor, playerName, kill_count, death_count, point);
        }
        else
        {
            Debug.Log("index out of range in local scoreboard");
        }
    }


    [ClientRpc]
    void RpcInitScoreboard()
    {
        

        if (NetworkGameManager.players.Count==scoreboardCells.Count)
            return;       


        for( int i = scoreboardCells.Count; i <NetworkGameManager.players.Count; i++  )
        {
            GameObject go = GameObject.Instantiate(scoreboard.gameObject);
            go.transform.SetParent(playerListRect, false);
            go.transform.SetAsLastSibling();
            scoreboardCells.Add(go.GetComponent<scoreboardCell>());
        }

    }




    // update scoreboard
    [ServerCallback]
    public void UpdateScoreboard()
    {
        NetworkGameManager.players.Sort( NetworkPlayer.compareByPoint );
        RpcUpdateScoreboard();
    }


    [ServerCallback]
    public void OnReceiveMessageFromClient(string text)
    {
        // boardcast to every client
        for(  int i = 0; i < players.Count; i++ )
        {
            chatScript chat = players[i].GetComponent<chatScript>();
            chat.RpcReceiveMessage(text);
        }
    }


      


    [ServerCallback]
    public void suicideRequest( GameObject player )
    {
        NetworkPlayer p = player.GetComponent<NetworkPlayer>();
        p.point--;
        p.death_count++;
        p.GetComponent<NetworkPlayer>().RpcGotHit("commit suicide!");
        UpdateScoreboard();
    }

    // player killed
    // bullet ----> network game manager ----> player
    [ServerCallback]
    public void PlayerKillBy( GameObject player, bullet hitBullet )
    {

        string killString = "killed by " + hitBullet.getOwnerName();
        NetworkPlayer p = player.GetComponent<NetworkPlayer>();
        if (player.name == hitBullet.getOwnerName())
        {
            killString = "You suicided!";
            p.point--;
        } else
        {
            NetworkPlayer killerPlayer = hitBullet.getOwner().GetComponent<NetworkPlayer>();
            killerPlayer.RpcKillNotify("You killed " + player.name);
            killerPlayer.point++;
            killerPlayer.kill_count++;

            if (!mResult)               
            {
                if ( killerPlayer.point >= winningPoints )
                {
                    OnReceiveMessageFromClient("////////////////////////");
                    OnReceiveMessageFromClient("Player : " + killerPlayer.name + " win");
                    OnReceiveMessageFromClient("////////////////////////");
                    mResult = true;
                }
            }

        }

        
        p.death_count++;
        p.RpcGotHit( killString );
        UpdateScoreboard();


        

    }


    public IEnumerator waitForRespawnTime( NetworkPlayer player )
    {
        yield return new WaitForSeconds(respawnTime);
        player.respawn();
    }

}
