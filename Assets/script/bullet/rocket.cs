using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class rocket : bullet {

    public float exploseRange = 5 ;
    public ParticleSystem m_explose_particle;

    void explose()
    {
        // detect all collider should be consider being hit.
        Collider[]  col = Physics.OverlapSphere( transform.position, exploseRange);
        // check hit
        for ( int i = 0; i < col.Length; i++ )
        {
            // ray cast to taget
            Ray ray = new Ray( transform.position, (col[i].transform.position - transform.position).normalized );
            RaycastHit hit;
            Physics.Raycast(ray, out hit, exploseRange);
            // if the first collider is the same. means direct hit.
            if (hit.collider == col[i])
            {
                Debug.Log("rocket hit : " + col[i].gameObject.name + "   tag : " + col[i].gameObject.tag);
                hitObject(col[i]);
            }
        }


        m_explose_particle.transform.parent = null;
        m_explose_particle.Play();
        Destroy(m_explose_particle, m_explose_particle.duration);

        //self destruct
        NetworkServer.Destroy(gameObject);

    }


    void hitObject( Collider collider )
    {
        //Debug.Log(gameObject.name + "  hit   " + collider.gameObject.name);
        if (collider.tag == "TreasureCase")
        {
            collider.GetComponent<treasureCase>().OpenCase();
        }
    }


    [ServerCallback]
    protected override void destroyBullet()
    {
        explose();
    }


    [ServerCallback]
    protected override void OnTriggerEnter( Collider collider )
    {

        explose();

    }


}
