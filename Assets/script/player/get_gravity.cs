using UnityEngine;
using System.Collections;

public class get_gravity : MonoBehaviour {

    public gravity_planet planet ;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    void FixedUpdate()
    {

        getGravity(planet);

    }

    void getGravity( gravity_planet planet )
    {
        planet.applyGravity(this.gameObject);
    }

}
