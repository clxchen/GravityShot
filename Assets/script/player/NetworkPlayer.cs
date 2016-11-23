using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {

    public fireControl fireCon;
    public playercontrol playerCon;
    public itemReceiver itemRec;
    public get_gravity m_gravity;


    Text killInfoText;
    Transform body;
    Vector3 originPosition;
    float upSpeed = 20f;

    [SyncVar]
    bool isDeath = false;

    Collider[] colliders;


	void Start () {
        body = transform.FindChild("body");
        colliders = GetComponentsInChildren<Collider>();
        killInfoText = GameObject.Find("killInfoText").GetComponent<Text>();
    }

    void Awake()
    {
        // add to player list
        NetworkGameManager.players.Add(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	    if ( isDeath )
        {
            body.localPosition += new Vector3(0,1,0) * upSpeed * Time.deltaTime;
        } else if ( Input.GetKeyDown(KeyCode.Q)) 
        {
            RpcGotHit( "suicide command!" );
           
            
        }
	}


    [ClientRpc]
    public void RpcGotHit( string killInfoString )
    {
        if( isLocalPlayer )
        {

            Death( killInfoString );
            StartCoroutine(NetworkGameManager.sInstance.waitForRespawnTime( this ));

        }
    }


    void Death( string killInfoString )
    {
        killInfoText.text = killInfoString;

        originPosition = body.localPosition;
        
        isDeath = true;

        // disable script when death
        fireCon.enabled = false;
        playerCon.enabled = false;
        itemRec.enabled = false;
        m_gravity.enabled = false;


        // disable collider
        for ( int i = 0; i < colliders.Length; i++ )
        {
            colliders[i].enabled = false;
        }


    }


    public void respawn()
    {
        if (isLocalPlayer)
        {
            isDeath = false;
            body.localPosition = originPosition;

            killInfoText.text = "";

            // reset transform information
            Transform spawnTrans = NetworkManager.singleton.GetStartPosition();
            transform.position = spawnTrans.position;
            transform.rotation = spawnTrans.rotation;

            // enable script
            fireCon.enabled = true;
            playerCon.enabled = true;
            itemRec.enabled = true;
            m_gravity.enabled = true;

            fireCon.useDefaultBullet();

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }


        }
    }



}
