using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class TreasureRespawn : NetworkBehaviour {

    private List<GameObject> treasureCases = new List<GameObject>();
    bulletCenter bulCenter;
    BulletType[] allBullet; 


	// Use this for initialization
	void Start () {

        bulCenter = GameObject.Find("BulletCenter").GetComponent<bulletCenter>();
        allBullet = bulCenter.GetAllBullet();

	    // search all treasure case in child
        for (int  i = 0; i < transform.childCount; i++ )
        {
            if (transform.GetChild(i).tag == "TreasureCase"  )
            {
                this.treasureCases.Add(transform.GetChild(i).gameObject);
            }
        }


        spawnAllCase();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void spawnAllCase()
    {
        for( int i = 0; i < treasureCases.Count; i++ )
        {
            treasureCase tCase = treasureCases[i].GetComponent<treasureCase>();
            if ( tCase)
            {
                tCase.setCaseContend(allBullet[Random.Range(0, allBullet.Length-1)]);
            }

        }
    }


}
