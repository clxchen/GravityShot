using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class treasureCase : NetworkBehaviour {

    private BulletType bulletItem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void setCaseContend( BulletType contend  )
    {
       
        bulletItem = contend;
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


        NetworkServer.Destroy(gameObject);
    }



}
