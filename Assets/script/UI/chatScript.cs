using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class chatScript : NetworkBehaviour {

    public Text chatText;
    public InputField inputF;
    public string playerName;


    void Start()
    {

        playerName = GetComponent<NetworkPlayer>().playerName;
        playerName = this.name;
        chatText = GameObject.Find("ChatroomText").GetComponent<Text>();
        inputF = GameObject.Find("chatroomInput").GetComponent<InputField>();
 
        inputF.onEndEdit.AddListener(OnInputEnter);
        /*
       for ( int i = 0; i < players.Length; i++)
        {
            if ( players[i].GetComponent<playercontrol>() )
            {
                playerName = players[i].name;
            }
        }
        */
    }

   
    public void OnInputEnter(string text)
    {
        
        CmdSendMessage(playerName + " : " + text);
    }

    [Command]
    public void CmdSendMessage(string text)
    {

        NetworkGameManager.sInstance.OnReceiveMessageFromClient(text);
    }



    [ClientRpc]
    public void RpcReceiveMessage(string message)
    {
        if ( isLocalPlayer )
            chatText.text += message + "\n";
    }


}
