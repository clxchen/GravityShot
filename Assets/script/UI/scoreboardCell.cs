using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class scoreboardCell : NetworkBehaviour {

    public Image playerColor;
    public Text playerName;
    public Text KillText;
    public Text DeathText;
    public Text pointText;
    


    public void UpdateScore( Color playerColor, string  playerName, int kill_count, int death_count, int point )
    {
        this.playerColor.color = playerColor;
        this.playerName.text = playerName;
        KillText.text = kill_count.ToString();
        DeathText.text = death_count.ToString();
        pointText.text = point.ToString();

        //NetworkPlayer p = NetworkGameManager.players[0].GetComponent<NetworkPlayer>();
    }

}
