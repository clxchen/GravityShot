using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {

    [SyncVar]
    public Color m_playerColor ;
    [SyncVar]
    public string m_playerName;

    public fireControl fireCon;
    public playercontrol playerCon;
    public itemReceiver itemRec;
    public get_gravity m_gravity;


    Transform body;
    Vector3 originPosition;
    float upSpeed = 10f;
    [SyncVar]
    bool isDeath = false;
    Collider[] colliders;


	void Start () {
        body = transform.FindChild("body");
        body.gameObject.GetComponent<Renderer>().material.color = m_playerColor;
        colliders = GetComponentsInChildren<Collider>();
    }

    void Awake()
    {
        NetworkGameManager.players.Add(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	    if ( isDeath )
        {
            body.localPosition += new Vector3(0,1,0) * upSpeed * Time.deltaTime;
        }
	}


    [ClientRpc]
    public void RpcGotHit( string playerName )
    {
        if( isLocalPlayer )
        {
            Death();
            StartCoroutine(NetworkGameManager.sInstance.waitForRespawnTime( this ));

        }
    }


    void Death()
    {
        originPosition = body.localPosition;
        isDeath = true;
        fireCon.enabled = false;
        playerCon.enabled = false;
        itemRec.enabled = false;
        m_gravity.enabled = false;
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

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }


            Transform spawnTrans = NetworkManager.singleton.GetStartPosition();
            transform.position = spawnTrans.position;
            transform.rotation = spawnTrans.rotation;
            fireCon.enabled = true;
            playerCon.enabled = true;
            itemRec.enabled = true;
            m_gravity.enabled = true;

        }
    }



}
