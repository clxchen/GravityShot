using UnityEngine;
using System.Collections;

public class playercontrol : MonoBehaviour {
    public float moveSpeed = 5f ;


    private int inputLayer ;

    Camera camera ;
    Vector3 m_moveDirection;

    Rigidbody rigid;

	// Use this for initialization
	void Start () {


        // only hit on input layer
        inputLayer = LayerMask.NameToLayer("Input");
        inputLayer = ~inputLayer;
        rigid = gameObject.GetComponentInParent<Rigidbody>();
        camera = Camera.main;
      


	}
	
	// Update is called once per frame
	void Update () {
      
        // look rotate by mouse
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f,inputLayer))
        {
            Vector3 relative = hit.point - transform.position;
            relative = transform.InverseTransformDirection(relative);
            relative.y = 0;

            float degree = Vector3.Angle(transform.InverseTransformDirection(transform.forward), relative);

            if (degree > 1f )
            {
                // cross method, if value less than zero means turn left 
                if (Vector3.Cross(transform.InverseTransformDirection(transform.forward), relative).y < 0)
                {
                    degree = -1*degree;
                    //Debug.Log(degree);
                }
                transform.Rotate(new Vector3(0, degree, 0));
                
            }
        }



       

        m_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        
    }   

    void FixedUpdate()
    {
        // move. depends on parent direction set up
        rigid.MovePosition(transform.position + transform.parent.transform.TransformDirection(m_moveDirection) * moveSpeed * Time.deltaTime);
    }
}
