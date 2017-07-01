using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomLobbyManager : NetworkLobbyManager
{
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        runInBackground = true;
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        SwitchToPlayerVIew();
    }

    public override void OnStartClient(NetworkClient lobbyClient)
    {
        base.OnStartClient(lobbyClient);
    }

    private void SwitchToPlayerVIew()
    {
        // When we have started ourself as a host we need to transition to the players view..
        var go = transform.Find("MainView").gameObject;
        go.SetActive(false);
        var po = transform.Find("PlayersView").gameObject;
        po.SetActive(true);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        base.OnLobbyClientConnect(conn);
    }

    private void SwitchToMainVIew()
    {
        // When we have started ourself as a host we need to transition to the players view..
        var go = transform.Find("PlayersView").gameObject;
        go.SetActive(false);
        var po = transform.Find("MainView").gameObject;
        po.SetActive(true);
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject lobbyPlayer = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;
        return lobbyPlayer;
    }

    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    base.OnServerAddPlayer(conn, playerControllerId);
    //}

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
    }
}
