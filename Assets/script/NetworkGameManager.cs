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


    public float respawnTime = 2f;

	
	void Awake () {
        //singleton
        sInstance = this ;
	}
	
	// Update is called once per frame
	void Update () { 
        if ( Input.GetKeyDown(KeyCode.Z) )
        {
            GameObject go = GameObject.Instantiate(scoreboard.gameObject);
            go.transform.SetParent(playerListRect, false);
        }

	}


    [ClientRpc]
    void RpcUpdateScoreboard()
    {
        // update scoreboard locally.



    }



    // update scoreboard
    [ServerCallback]
    void UpdateScoreboard()
    {
        NetworkGameManager.players.Sort( NetworkPlayer.compareByPoint );
        RpcUpdateScoreboard();
    }

   


    [ServerCallback]
    public void suicideRequest( GameObject player )
    {
        player.GetComponent<NetworkPlayer>().RpcGotHit("commit suicide!");
    }

    // player killed
    // bullet ----> network game manager ----> player
    [ServerCallback]
    public void PlayerKillBy( GameObject player, bullet hitBullet )
    {

        string killString = "killed by " + hitBullet.getOwnerName();
        if (player.name == hitBullet.getOwnerName())
            killString = "You suicided!";

        

        NetworkPlayer killPlayer = hitBullet.getOwner().GetComponent<NetworkPlayer>();
        killPlayer.RpcKillNotify("You killed " + player.name);
        player.GetComponent<NetworkPlayer>().RpcGotHit( killString );

    }


    public IEnumerator waitForRespawnTime( NetworkPlayer player )
    {
        yield return new WaitForSeconds(respawnTime);
        player.respawn();
    }

}
