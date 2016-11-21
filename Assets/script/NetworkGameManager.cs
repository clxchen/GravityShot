using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkGameManager : NetworkBehaviour {

    static public NetworkGameManager sInstance ;
    static public List<GameObject> players = new List<GameObject>();
    public float respawnTime = 2f;


	
	void Awake () {
        sInstance = this ;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void PlayerKillBy( GameObject player, bullet hitBullet )
    {
        player.GetComponent<NetworkPlayer>().RpcGotHit( hitBullet.getOwnerName() );

    }


    public IEnumerator waitForRespawnTime( NetworkPlayer player )
    {
        yield return new WaitForSeconds(respawnTime);
        player.respawn();
    }

}
