using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class treasureCase : NetworkBehaviour {


    public delegate void respawnerRespond(int number) ;
    public respawnerRespond responder ;

    private BulletType bulletItem;
    private int number;

	// Use this for initialization
	void Start () {
	
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
            go.transform.position = this.transform.position;
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
