using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HUX.Receivers;
using HUX.Interaction;
using HUX.Focus;

public class Receivers : InteractionReceiver
{
    public GameObject TextObjectState;
    public TextMesh txt;

    void Start()
    {
        //txt = TextObjectState.GetComponentInChildren<TextMesh>();
    }

    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        //txt.text = obj.name + " : OnTapped";

        switch (obj.name)
        {
            case "HostButton":
                {
                    // Not sure this is the best way to do this.. seems wrong.. 
                    var trgt = Targets[0].GetComponent<HoloLensLobbyManager>();
                    var nc = trgt.StartHost();
                    Debug.Log("server ip = " + nc.serverIp);
                    Debug.Log("server port = " + nc.serverPort);
                    break;
                }
            case "ClientButton":
                {
                    var trgt = Targets[0].GetComponent<HoloLensLobbyManager>();
                    trgt.networkAddress = "192.168.0.10";// "192.168.0.10"; //ipInput.text;
                    trgt.networkPort = 7777;

                    var nc = trgt.StartClient();
                    trgt.IsClientConnected();
                    break;
                }
        }
        base.OnTapped(obj, eventArgs);
    }

    protected override void OnHoldStarted(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        //txt.text = obj.name + " : OnHoldStarted";
        base.OnHoldStarted(obj, eventArgs);
    }

    protected override void OnFocusEnter(GameObject obj, FocusArgs args)
    {
        //txt.text = obj.name + " : OnFocusEnter";
        base.OnFocusEnter(obj, args);
    }

    protected override void OnFocusExit(GameObject obj, FocusArgs args)
    {
        //txt.text = obj.name + " : OnFocusExit";
        base.OnFocusExit(obj, args);
    }
}
