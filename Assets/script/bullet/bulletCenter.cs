﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


[System.Serializable]
public class BulletType
{
    public GameObject prefab;
    public string tag ;
    public int maxBulletInQueue;
    [HideInInspector]
    public Queue<GameObject> pool = new Queue<GameObject>() ;
}

public class bulletCenter :NetworkBehaviour  {

    public int num_player = 1;
    private List<Queue<GameObject>> m_objPools;
    public BulletType[] allPrefabs;

    // need to add tag after custom editor complete!
    public override void OnStartServer()
    {

        CmdGenerateBullet();
        
    }

 
    void CmdGenerateBullet()
    {
        m_objPools = new List<Queue<GameObject>>();
        for (int i = 0; i < allPrefabs.Length; i++)
        {
            Queue<GameObject> tmp = new Queue<GameObject>();
            allPrefabs[i].pool = tmp;
            for (int j = 0; j < allPrefabs[i].maxBulletInQueue; j++)
            {
                GameObject obj = null;
                createBullet(allPrefabs[i], out obj);
                
            }

            m_objPools.Add(tmp);
        }
    }



    [Command]
    public void CmdFireBullet( string type )
    {
        for (int i = 0; i < allPrefabs.Length; i++)
        {
            if (allPrefabs[i].tag == type)
            {
                GameObject obj = null;
                if ( allPrefabs[i].pool.Count == 0 )
                {
                    
                    createBullet(allPrefabs[i], out obj);
                    
                }

                obj = allPrefabs[i].pool.Dequeue();
                bullet bul = obj.GetComponent<bullet>();
               // bul.CmdfireInit();
            }

        }
    }



    // generate a bullet from bulletType
    void createBullet( BulletType type, out GameObject bullet )
    {
        bullet = GameObject.Instantiate(type.prefab);
        bullet.GetComponent<bullet>().setTag(type.tag);
        NetworkServer.Spawn(bullet);
        bullet.SetActive(false);
        type.pool.Enqueue(bullet);
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





    
    public void getBullet(string str, out GameObject obj)
    {
        for ( int i = 0; i< allPrefabs.Length; i++ )
        {
            if ( allPrefabs[i].tag == str )
            {
                createBullet(allPrefabs[i], out obj);
                return;
            }

        }

        // something wrong!!
        createBullet(allPrefabs[0], out obj);
    }

    public void getBullet(GameObject bulletType, out GameObject obj)
    {
        for (int i = 0; i < allPrefabs.Length; i++)
        {
            if (allPrefabs[i].tag == bulletType.GetComponent<bullet>().getTag())
            {
                createBullet(allPrefabs[i], out obj);
                return;
            }

        }

        // something wrong!!
        createBullet(allPrefabs[0], out obj);

    }


    public void getBulletPool( string str, out Queue<GameObject> pool )
    {
        for (int i = 0; i < allPrefabs.Length; i++)
        {
            if (allPrefabs[i].tag == str )
            {
                pool = allPrefabs[i].pool ;
                return;
            }
        }

        // something wrong!!
        pool = m_objPools[0];
    }

    public void getBulletPool( GameObject obj, out Queue<GameObject> pool ) 
    {
        for ( int i = 0; i < allPrefabs.Length; i++ )
        {
            if ( allPrefabs[i].tag == obj.GetComponent<bullet>().getTag() )
            {
                pool = allPrefabs[i].pool;
                return;
            }
        }

        // something wrong!!
        pool = m_objPools[0];
    }





	
}
