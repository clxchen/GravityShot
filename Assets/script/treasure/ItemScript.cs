using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemScript : NetworkBehaviour {


    public float liftTime = 10f;

    BulletType bulletItem;
    private float timer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= liftTime) NetworkServer.Destroy(gameObject);
	}

    public void setAttribute( BulletType type )
    {
        bulletItem = type;        
    }




    // pick up
   [ServerCallback]
    void OnTriggerEnter(  Collider  collider )
    {
        // player
        if ( collider.tag == "Player" )
        {
            itemReceiver receiver = collider.gameObject.transform.parent.GetComponent<itemReceiver>();
            if( receiver )
            {
                receiver.RpcReceiveItem(bulletItem);
                NetworkServer.Destroy(gameObject);
            }

        }


    }




}
