using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class playerNetworkSetting : NetworkBehaviour {
    // network player object setting
    [SyncVar]
    public Color m_playerColor;
    [SyncVar]
    public string m_playerName;



    private playercontrol playercontrl;
    private Transform cameraTrans;
    private Camera camera;
    private AudioListener audioListener;
    private Collider inputCol;
    private fireControl fireCon;
    private NetworkPlayer networkPlayer;

	// Use this for initialization
	void Start () {

        gameObject.name = m_playerName;
        transform.FindChild("body").gameObject.GetComponent<Renderer>().material.color = m_playerColor;




        playercontrl = gameObject.GetComponentInChildren<playercontrol>();
        cameraTrans = transform.FindChild("Camera");
        camera = cameraTrans.GetComponent<Camera>();
        audioListener = cameraTrans.GetComponent<AudioListener>();
        fireCon = gameObject.GetComponentInChildren<fireControl>();
        inputCol = transform.FindChild("inputCollider").GetComponent<Collider>() ;
        networkPlayer = GetComponent<NetworkPlayer>();

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
            if (networkPlayer)
                networkPlayer.enabled = false;

        }


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
