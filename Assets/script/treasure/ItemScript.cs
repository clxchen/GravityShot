using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemScript : NetworkBehaviour {


    public float liftTime = 10f;
    public float rotatePerSecond = 60f;
    public float float_Height = 0.1f;
    public float float_speed = 3 ;

    BulletType bulletItem;
    private float timer = 0;
    private Vector3 m_startPosition;




	// Use this for initialization
	void Start () {
        m_startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= liftTime) NetworkServer.Destroy(gameObject);

        transform.position = (Mathf.Sin(timer*float_speed) * float_Height) * transform.up + m_startPosition;
        transform.Rotate(0, rotatePerSecond * Time.deltaTime, 0);
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
