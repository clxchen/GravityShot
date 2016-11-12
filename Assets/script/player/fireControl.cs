using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// shot bullets
public class fireControl : MonoBehaviour {

    public float fire_rate = 1 / 10  ;


    private bulletCenter m_bulletCenter ;
    BulletType m_bulletType;
  

    float timer = 99 ;
    // Use this for initialization
    void Start () {
        m_bulletCenter = GameObject.Find("BulletCenter").GetComponent<bulletCenter>();
        m_bulletType = m_bulletCenter.getBulletType("cube");
    }
	
	// Update is called once per frame
	void Update () {
        
	    if ( timer > fire_rate&& Input.GetMouseButton(0))
        {
            fire();
            timer = 0;
        }

        timer += Time.deltaTime;
    }


    public void fire()
    {
        
        GameObject obj = null;
        // get one avaible bullet
        if ( m_bulletType.pool.Count == 0)  // no bullet! create one
        {
            m_bulletCenter.getBullet(m_bulletType.tag, out obj);
        }else
        {
            obj = m_bulletType.pool.Dequeue();
        }


        // handle fire procedure
        if ( obj != null )
        {
            bullet bul = obj.GetComponent<bullet>();
            bul.fireInit(transform.position+transform.up*0.1f, transform.forward);
            bul.setOwner(this.gameObject);


        }else // something wrong!!
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
