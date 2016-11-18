using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


[System.Serializable]
public class BulletType
{
    public GameObject prefab;
    public GameObject item ;
    public string tag ;
    public float fire_rate;
    public int maxBullet;
}

public class bulletCenter :NetworkBehaviour  {

    public int num_player = 1;
    

   
    public BulletType[] allPrefabs;

    // need to add tag after custom editor complete!
    public override void OnStartServer()
    {
        
    }


    
    public BulletType[] GetAllBullet()
    {
        return allPrefabs;
    }

    public BulletType getBulletType( string str )
    {
        for (int i = 0; i < allPrefabs.Length; i++)
        {
            if (allPrefabs[i].tag == str)
            {
                
                return allPrefabs[i] ;
            }

        }

        return allPrefabs[0];
    }



	
}
