using UnityEngine;
using System.Collections;

public class gravity_planet : MonoBehaviour {


    public float force_const = 9.8f;
    
	// Use this for initialization
	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    // make object attract by planet 
    //      obj needs Rigidbody commponent to make this work
    public void applyGravity( GameObject obj )
    {
       
        Vector3 direction = (obj.transform.position - this.transform.position).normalized;

        // add force from direction 
        obj.GetComponent<Rigidbody>().AddForce( -direction * force_const );

        // adjust object rotation to "looks" normally
        Quaternion targetRotation = Quaternion.FromToRotation(obj.transform.up, direction )*obj.transform.rotation;
        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, targetRotation,Time.deltaTime*100);
    }

}
