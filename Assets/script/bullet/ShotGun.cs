using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShotGun : bullet {


    public GameObject cellBulletPrefab;
    public int cell_amount ;
    public float angle ;


    public override void CmdfireInit(Vector3 position, Vector3 forwardRotation)
    {
        centerObj = GameObject.Find("planet").transform;
        for ( int i = 0;  i < cell_amount; i++ )
        {

           

            float offset = (( i  - (cell_amount / 2f)) / cell_amount) * angle ;
            //Debug.Log(i + " :  " + offset.ToString());
            if ( i == 19 )
            {
                ;
            }
            GameObject obj = GameObject.Instantiate(cellBulletPrefab);
            bullet bul = obj.GetComponent<bullet>();
            NetworkServer.Spawn(obj);    
            bul.CmdfireInit(position, forwardRotation);

            bul.transform.RotateAround( centerObj.position- bul.transform.position, offset );
            //bul.transform.Rotate(0, offset, 0);
            bul.setOwner(this.gameObject);

            NetworkServer.Destroy(gameObject);
        }
    }
}
