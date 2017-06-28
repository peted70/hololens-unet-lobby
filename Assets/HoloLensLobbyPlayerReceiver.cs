using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HUX.Receivers;
using HUX.Interaction;
using HUX.Focus;

public class HoloLensLobbyPlayerReceiver : InteractionReceiver 
{
    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        //txt.text = obj.name + " : OnTapped";

        switch (obj.name)
        {
            case "JoinButton":
                {
                    // Not sure this is the best way to do this.. seems wrong.. 
                    var trgt = Targets[0].GetComponent<HoloLensLobbyPlayer>();
                    trgt.SendReadyToBeginMessage();
                    break;
                }
            case "ColourButton":
                {
                    // Not sure this is the best way to do this.. seems wrong.. 
                    var trgt = Targets[0].GetComponent<HoloLensLobbyPlayer>();
                    trgt.CmdColorChange();
                    break;
                }
        }
    }
}