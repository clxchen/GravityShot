using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class bullet : NetworkBehaviour {


   
    public float m_speed = 5f;
    public Transform centerObj;         // fly around with this object

    // owner of this bullet
    private GameObject owner = null ;
    private string tag = "";


	// Use this for initialization
	void Start () {
        centerObj = GameObject.Find("planet").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            planetMode();
        }


    }

    // makes bullet fly around the planet
    void planetMode()
    {
        Vector3 centerToThis = transform.position - centerObj.position;
        float radius = Mathf.Abs((transform.position - centerObj.position).sqrMagnitude);
        float percentOfDistance =   ((m_speed * Time.deltaTime) /
                                      (2 * radius * Mathf.PI)) ;

        float degreeToTurn = percentOfDistance * 360;

        // use cross product to create axis 
        transform.RotateAround(centerObj.position, Vector3.Cross(centerToThis, transform.forward), degreeToTurn);


    }

   


    public bool fireable()
    {
        

        return false;
    }


    // fire init
    public void CmdfireInit( Vector3 position, Vector3 forwardRotation )
    {
        gameObject.SetActive(true);
        transform.forward = forwardRotation;
        transform.position = position;

    }

    public void checkLifeCicleEnd()
    {

    }


    // switch center object
    public void setCenterTarget( Transform trans )
    {

        this.centerObj = trans;

    }

    // go back to pool
    public void recovery()
    {
        //owner.GetComponent<fireControl>().recoverBullet( this.gameObject );

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


    void OnTriggerEnter( Collider collider )
    {
        Debug.Log( gameObject.name + "  hit   "  + collider.gameObject.name );



        Destroy(gameObject);

    }

}
