using HUX.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomLobbyManager : NetworkLobbyManager
{
    public ObjectCollection playerCollection;

    public override void OnStartHost()
    {
        base.OnStartHost();

        // When we have started ourself as a host we need to transition to the players view..
        var go = transform.Find("MainView").gameObject;
        go.SetActive(false);
        var po = transform.Find("PlayersView").gameObject;
        po.SetActive(true);
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject lobbyPlayer = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

        var cn = new ObjectCollection.CollectionNode();
        cn.transform = lobbyPlayer.transform;
        playerCollection.NodeList.Add(cn);
        playerCollection.UpdateCollection();
        return lobbyPlayer;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
