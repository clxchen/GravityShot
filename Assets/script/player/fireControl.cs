using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

// shot bullets
public class fireControl : NetworkBehaviour {

    public float fire_rate = 1 / 10;
    public Transform bodyTrans;


    public bulletCenter m_bulletCenter;
    public BulletType m_bulletType;

    Text bulletCountText;
    private string defaultBullet = "normal";


    float timer = 0;
    private int bulletCount = 0;

    // Use this for initialization
    void Awake() {

        if (isLocalPlayer)
            m_bulletCenter = GameObject.Find("BulletCenter").GetComponent<bulletCenter>();


        m_bulletType = m_bulletCenter.getBulletType(defaultBullet);
        bulletCountText = GameObject.Find("bulletCountText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {


        if (m_bulletType == null)
            m_bulletType = m_bulletCenter.getBulletType(defaultBullet);

        if (timer > m_bulletType.fire_rate && Input.GetMouseButton(0))
        {
            CmdFire(m_bulletType.tag);
            timer = 0;
            bulletCount++;


            // check bullet amount
            // -1 means limitless
            if (bulletCount >= m_bulletType.maxBullet && m_bulletType.maxBullet != -1)
            {
                // reset to basic bullet
                m_bulletType = m_bulletCenter.getBulletType(defaultBullet);
                bulletCount = 0;
            }
        }

        if (m_bulletType != null)
        {
            if (m_bulletType.maxBullet != -1)
                bulletCountText.text = (m_bulletType.maxBullet - bulletCount) + " / " + m_bulletType.maxBullet.ToString();
            else
                bulletCountText.text = " - / - ";

        } else
        {
            bulletCountText.text = "";
        }


        timer += Time.deltaTime;
    }


    [Command]
    void CmdFire(string bulletStr)
    {

        GameObject obj = null;

        obj = GameObject.Instantiate(m_bulletCenter.getBulletType(bulletStr).prefab);


        // handle fire procedure
        if (obj != null)
        {


            bullet bul = obj.GetComponent<bullet>();

            bul.CmdfireInit(bodyTrans.transform.position + bodyTrans.transform.up * 0.1f + bodyTrans.transform.forward * 0.3f, bodyTrans.transform.forward, gameObject);
            //bul.setOwner(this.gameObject);
            NetworkServer.Spawn(obj);

        }
        else // something wrong!!
        {
            Debug.Log("no bullet to shot!!");
        }



    }


    public void useDefaultBullet()
    {
        if (m_bulletCenter)
        {
            m_bulletType = m_bulletCenter.getBulletType(defaultBullet);
        }
    }

    
    public void getBullet( BulletType type )
    {
        this.m_bulletType = type;
        bulletCount = 0;
    }

}
