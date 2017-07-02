using HUX.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class HoloLensLobbyPlayer : NetworkLobbyPlayer
{
    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    //used on server to avoid assigning the same color to two player
    static List<int> _colorInUse = new List<int>();

    public TextMesh statusText;
    public TextMesh nameText;

    public ObjectCollection playerCollection;

    //OnMyName function will be invoked on clients when server change the value of playerName
    [SyncVar(hook = "OnMyName")]
    public string playerName = "";
    [SyncVar(hook = "OnMyColor")]
    public Color playerColor = Color.white;

    //public override void OnStartLocalPlayer()
    //{
    //    base.OnStartLocalPlayer();
    //}

    //public override void OnStartAuthority()
    //{
    //    base.OnStartAuthority();
    //    SetupLocalPlayer();
    //}

    //public override void OnClientEnterLobby()
    //{
    //    base.OnClientEnterLobby();

    //    PlayerList.instance.AddPlayer(this);

    //    //var lobbyPlayer = this;

    //    //// Need to clone the lobby player prefab here at this point..
    //    //var cn = new ObjectCollection.CollectionNode();
    //    //cn.transform = lobbyPlayer.transform;
    //    //lobbyPlayer.transform.parent = playerCollection.transform;
    //    //playerCollection.NodeList.Add(cn);
    //    //playerCollection.UpdateCollection();

    //    if (isLocalPlayer)
    //    {
    //        SetupLocalPlayer();
    //    }
    //    else
    //    {
    //        SetupOtherPlayer();
    //    }

    //    // setup the player data on UI. The value are SyncVar so the player
    //    // will be created with the right value currently on server
    //    OnMyName(playerName);
    //    OnMyColor(playerColor);
    //}

    private void SetupOtherPlayer()
    {
        //throw new NotImplementedException();
    }

    private void SetupLocalPlayer()
    {
        if (playerColor == Color.white)
            CmdColorChange();
        if (playerName == "")
            CmdNameChanged("Player" + this.netId.ToString());
    }

    //public override void OnClientReady(bool readyState)
    //{
    //    base.OnClientReady(readyState);
    //    statusText.text = readyState ? "Ready" : "Not Ready";
    //}

    public void OnMyName(string newName)
    {
        playerName = newName;
        nameText.text = playerName;
    }

    public void OnMyColor(Color newColor)
    {
        playerColor = newColor;

        // change the tint colour of the HoloLens models visor
        var deviceGo = gameObject.GetComponentsInChildren<Transform>();
        var device = deviceGo.Where(t => t.name == "hololens-lobby-player").Single() as Transform;
        var mr = device.GetComponents<MeshRenderer>();
        foreach (var mat in mr[0].materials)
        {
            if (mat.name.Contains("glass"))
            {
                mat.color = playerColor;
                break;
            }
        }
    }

    [Command]
    public void CmdColorChange()
    {
        int idx = System.Array.IndexOf(Colors, playerColor);
        int inUseIdx = _colorInUse.IndexOf(idx);

        if (idx < 0) idx = 0;
        idx = (idx + 1) % Colors.Length;

        bool alreadyInUse = false;

        do
        {
            alreadyInUse = false;
            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    alreadyInUse = true;
                    idx = (idx + 1) % Colors.Length;
                }
            }
        }
        while (alreadyInUse);

        if (inUseIdx >= 0)
        {//if we already add an entry in the colorTabs, we change it
            _colorInUse[inUseIdx] = idx;
        }
        else
        {//else we add it
            _colorInUse.Add(idx);
        }

        playerColor = Colors[idx];
    }

    [Command]
    public void CmdNameChanged(string name)
    {
        playerName = name;
    }

    [ClientRpc]
    public void RpcUpdateCountdown(int countdown)
    {
        HoloLensLobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
        HoloLensLobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
    }
}
