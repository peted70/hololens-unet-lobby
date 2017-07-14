using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HoloLensLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        var lPlayer = lobbyPlayer.GetComponent<HoloLensLobbyPlayer>();
        var gPlayer = gamePlayer.GetComponent<HoloLensPlayerScript>();

        if (lPlayer && gPlayer)
        {
            gPlayer.PlayerColour = lPlayer.playerColor;
            gPlayer.PlayerName = lPlayer.playerName;
        }
    }
}
