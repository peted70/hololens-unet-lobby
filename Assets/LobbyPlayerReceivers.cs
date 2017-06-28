using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HUX.Receivers;
using HUX.Interaction;
using HUX.Focus;

public class LobbyPlayerReceivers : InteractionReceiver
{
    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        //txt.text = obj.name + " : OnTapped";
        switch (obj.name)
        {
            case "JoinButton":
                {
                    var lobbyPlayer = gameObject.GetComponent<HoloLensLobbyPlayer>();
                    lobbyPlayer.SendReadyToBeginMessage();
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
