using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

// shot bullets
public class fireControl : NetworkBehaviour {

    public float fire_rate = 1 / 10  ;
    public Transform bodyTrans ;


    public bulletCenter m_bulletCenter ;
    public BulletType m_bulletType;
  

    float timer = 99 ;
    // Use this for initialization
    void Start() {
        if (isLocalPlayer)
            m_bulletCenter = GameObject.Find("BulletCenter").GetComponent<bulletCenter>();

        m_bulletType = m_bulletCenter.getBulletType("cube");
    }
	
	// Update is called once per frame
	void Update () {
        
	    if ( timer > fire_rate&& Input.GetMouseButton(0))
        {
            CmdFire(m_bulletType.tag);
            timer = 0;
        }

        timer += Time.deltaTime;
    }


    [Command]
    void CmdFire( string bulletStr )
    {
        
        GameObject obj = null;
        /*
        m_bulletType = m_bulletCenter.getBulletType(bulletStr);
        // get one avaible bullet
        if ( m_bulletType.pool.Count == 0)  // no bullet! create one
        {
            m_bulletCenter.getBullet(m_bulletType.tag, out obj);
            obj = m_bulletType.pool.Dequeue();
            Debug.Log("object pool null");

        }else
        {
            obj = m_bulletType.pool.Dequeue();
            Debug.Log("get bullet from object pool");
        }
        */


        obj = GameObject.Instantiate(m_bulletCenter.getBulletType(bulletStr).prefab);


        // handle fire procedure
        if ( obj != null )
        {

            
            bullet bul = obj.GetComponent<bullet>();
            NetworkServer.Spawn(obj);
            bul.CmdfireInit(bodyTrans.transform.position+ bodyTrans.transform.up*0.1f+ bodyTrans.transform.forward*0.3f, bodyTrans.transform.forward);
            bul.setOwner(this.gameObject);
            

        }
        else // something wrong!!
        {
            Debug.Log( "no bullet to shot!!" );
        }
        


    }

    public void getPool(  )
    {

        m_bulletCenter.getBulletPool(m_bulletType.tag,out m_bulletType.pool);
        Debug.Log(m_bulletType.pool.Count);
    }


}
