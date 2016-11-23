using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BigBullet : bullet {


    float rotateSpeedPerSecond = 180f;
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();           
        transform.Rotate(new Vector3(0, 0, rotateSpeedPerSecond * Time.deltaTime));

	}
    
    [ServerCallback]
    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "TreasureCase")
        {
            collider.GetComponent<treasureCase>().OpenCase();
        } else if (collider.tag == "Player")
        {
            // notify game manager
            NetworkGameManager.sInstance.PlayerKillBy(collider.transform.parent.gameObject, this);
            NetworkServer.Destroy(gameObject);
        } else if ( collider.tag == "Bullet" ) 
        {
            // destroy bullet
            NetworkServer.Destroy(collider.gameObject);
        }


        
    }

}
