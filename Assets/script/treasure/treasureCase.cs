using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class treasureCase : NetworkBehaviour {


    public delegate void respawnerRespond(int number) ;
    public respawnerRespond responder ;
    public float item_Height_offset = 0.4f;

    private BulletType bulletItem;
    private int number;

	// Use this for initialization
    [ServerCallback]
	void Start () {
        BoxCollider bx = GetComponent<BoxCollider>();
        Collider[] colliders =  Physics.OverlapSphere( transform.position,  0.2f );

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<ItemScript>())
            {
                NetworkServer.Destroy(colliders[i].gameObject);
                Debug.Log(colliders[i].name);
            }


            
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void setCaseContend( BulletType contend   )
    {
       
        bulletItem = contend;
    }

    public void setNumber( int number )
    {
        this.number = number;
    }



    // open case to show item inside the case
    [ServerCallback]
    public void OpenCase()
    {
        if (bulletItem != null) {
            GameObject go = GameObject.Instantiate(bulletItem.item);
            go.transform.position = this.transform.position + transform.up * item_Height_offset;
            go.transform.rotation = this.transform.rotation;

            NetworkServer.Spawn(go);
            
            ItemScript item = go.GetComponent<ItemScript>();
            if ( item != null ) {
                item.setAttribute(bulletItem);                
            }
        }

        responder(number);
        Debug.Log("destroyed");
        NetworkServer.Destroy(this.gameObject);
    }



   


}
