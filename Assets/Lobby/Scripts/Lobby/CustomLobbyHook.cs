using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class CustomLobbyHook : LobbyHook {


    // pass data from lobby to game
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {

        LobbyPlayer lbPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();

        playercontrol playerCon = gamePlayer.GetComponentInChildren<playercontrol>();


        playerCon.m_playerColor = lbPlayer.playerColor;
        playerCon.player_name = lbPlayer.playerName;


    }
}
