using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class playerNetworkSetting : NetworkBehaviour {
    // network player object setting

    private playercontrol playercontrl;
    private Transform cameraTrans;
    private Camera camera;
    private AudioListener audioListener;
    private Collider inputCol;
    private fireControl fireCon;

	// Use this for initialization
	void Start () {

        playercontrl = gameObject.GetComponentInChildren<playercontrol>();
        cameraTrans = transform.FindChild("Camera");
        camera = cameraTrans.GetComponent<Camera>();
        audioListener = cameraTrans.GetComponent<AudioListener>();
        fireCon = gameObject.GetComponentInChildren<fireControl>();
        inputCol = transform.FindChild("inputCollider").GetComponent<Collider>() ;


        if ( !isLocalPlayer )
        {
            if (playercontrl)
                playercontrl.enabled = false;

            if (camera)
                camera.enabled = false;

            if (audioListener)
                audioListener.enabled = false;

            if (inputCol)
                inputCol.enabled = false;

            if (fireCon)
                fireCon.enabled = false;

        }


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
