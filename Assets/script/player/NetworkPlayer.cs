using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {

    public fireControl fireCon;
    public playercontrol playerCon;
    public itemReceiver itemRec;
    public get_gravity m_gravity;
    public AudioSource DeathAudio;
    public AudioSource killAudio;

    Rigidbody rigid;

    Text killInfoText;
    Text killText;
    Transform body;
    Vector3 originPosition;
    float upSpeed = 20f;
    float killTextTimer ;


    [SyncVar]
    bool isDeath = false;

    Collider[] colliders;


	void Start () {
        body = transform.FindChild("body");
        colliders = GetComponentsInChildren<Collider>();
        killInfoText = GameObject.Find("killInfoText").GetComponent<Text>();
        killText = GameObject.Find("killText").GetComponent<Text>();
        StartCoroutine(killTextUI());
        rigid = GetComponent<Rigidbody>();
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
            CmdSuicideRequest();
           
            
        }
	}


    [Command]
    void CmdSuicideRequest()
    {
        NetworkGameManager.sInstance.suicideRequest(gameObject);
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

    [ClientRpc]
    public void RpcKillNotify( string killInfoString )
    {
        if ( isLocalPlayer  )
        {
            killText.text = killInfoString ;
            killTextTimer = Time.time + 2f ;
            if (killAudio)
                killAudio.Play();
        }
    }


    IEnumerator killTextUI()
    {
        for ( float timer = Time.time ; true;  timer+=Time.deltaTime )
        {

            if (timer > killTextTimer)
                killText.text = "";

            yield return Time.deltaTime;
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

        if (DeathAudio)
            DeathAudio.Play();

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

            // reset physics
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            

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
