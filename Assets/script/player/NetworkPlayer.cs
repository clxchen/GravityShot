using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {

    public Color m_playerColor ;
    public string m_playerName;


	void Start () {
        transform.FindChild("body").gameObject.GetComponent<Renderer>().material.color = m_playerColor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    [ClientRpc]
    public void RpcGotHit()
    {
        if( isLocalPlayer )
        {
            Transform spawnTrans = NetworkManager.singleton.GetStartPosition();
            transform.position = spawnTrans.position;
            transform.rotation = spawnTrans.rotation;

        }
    }



}
