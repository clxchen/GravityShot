using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class bullet : NetworkBehaviour {


   
    public float m_speed = 5f;
    public Transform centerObj;         // fly around with this object
    public float range ;

    // owner of this bullet
    private GameObject owner = null ;
    private string tag = "";
    private Vector3 startPos;
    private float distant = 0 ;


	// Use this for initialization
	void Start () {
       
        centerObj = GameObject.Find("planet").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            planetMode();
            distant += m_speed *Time.deltaTime ;

            if ( distant >= range )
            {
                destroyBullet();
            }

        }


    }

    // makes bullet fly around the planet
    protected virtual void planetMode()
    {
        Vector3 centerToThis = transform.position - centerObj.position;
        float radius = Vector3.Distance(transform.position, centerObj.position);

        // percent of circumference in current frame with m_speed
        float percentOfCircumference =   ((m_speed * Time.deltaTime) /
                                      (2 * radius * Mathf.PI)) ;

        float degreeToTurn = percentOfCircumference * 360;

        Vector3 ps = transform.position;
        // use cross product to create axis 
        transform.RotateAround(centerObj.position, Vector3.Cross(centerToThis, transform.forward), degreeToTurn);
        

    }

   


    public bool fireable()
    {
        

        return false;
    }


    // fire init
    public virtual void CmdfireInit( Vector3 position, Vector3 forwardRotation )
    {
        gameObject.SetActive(true);
        transform.forward = forwardRotation;
        transform.position = position;
        startPos = position;
        distant = 0;
    }

    public void checkLifeCicleEnd()
    {

    }


    // switch center object
    public void setCenterTarget( Transform trans )
    {

        this.centerObj = trans;

    }


    public void setOwner( GameObject owner )
    {

        this.owner = owner;

    }


    public void setTag( string str )
    {
        this.tag = str;

    }

    public string getTag() { return this.tag; }


    [ServerCallback]
    protected virtual void destroyBullet()
    {
        NetworkServer.Destroy(gameObject);
    }

    // hit
    [ServerCallback]
    protected virtual void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(gameObject.name + "  hit   " + collider.gameObject.name);
        if (collider.tag == "TreasureCase") {
            collider.GetComponent<treasureCase>().OpenCase();
        }


        NetworkServer.Destroy(gameObject);

    }

}
