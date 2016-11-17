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


    private string defaultBullet = "normal";


    float timer = 99 ;
    private int bulletCount = 0;

    // Use this for initialization
    void Start() {
        if (isLocalPlayer)
            m_bulletCenter = GameObject.Find("BulletCenter").GetComponent<bulletCenter>();

        m_bulletType = m_bulletCenter.getBulletType(defaultBullet);
    }
	
	// Update is called once per frame
	void Update () {
        
	    if ( timer > m_bulletType.fire_rate&& Input.GetMouseButton(0))
        {
            CmdFire(m_bulletType.tag);
            timer = 0;
            bulletCount++;


            // check bullet amount
            // -1 means limitless
            if ( bulletCount >= m_bulletType.maxBullet && m_bulletType.maxBullet != -1 )
            {
                // reset to basic bullet
                m_bulletType = m_bulletCenter.getBulletType(defaultBullet);
                bulletCount = 0;
            }
        }

        timer += Time.deltaTime;
    }


    [Command]
    void CmdFire( string bulletStr )
    {
        
        GameObject obj = null;

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
    
    public void getBullet( BulletType type )
    {
        this.m_bulletType = type;
        bulletCount = 0;
    }

}
