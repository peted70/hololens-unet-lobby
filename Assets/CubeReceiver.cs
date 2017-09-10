using HUX.Interaction;
using HUX.Receivers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.VR.WSA;

public class CubeReceiver : InteractionReceiver
{
    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        switch (obj.name)
        {
            case "Cube":
                {
                    // Find the local player object...
                    var scripts = GameObject.FindObjectsOfType<HoloLensPlayerScript>();
                    if (scripts == null)
                        Debug.Log("Scripts is null");
                    HoloLensPlayerScript localPlayer = new List<HoloLensPlayerScript>(scripts).Find(player => player.isLocalPlayer);
                    //if (localPlayer != null)
                    //{
                    var sharedCollection = GameObject.Find("SharedCollection");
                    if (sharedCollection && localPlayer)
                    { 
#if WINDOWS_UWP
                        //WorldAnchorMgr.Instance.ImportWorldAnchorFromDisk();
                        //break;
#endif
                        var was = localPlayer.GetComponent<WorldAnchorScript>();
                        var wa = sharedCollection.GetComponent<WorldAnchor>();
                        if (wa == null)
                            wa = sharedCollection.AddComponent<WorldAnchor>();
                        Logger.Log("About to upload world anchor");
                        was.UploadWorldAnchor(wa);
                    }
                    break;
                }
        }
    }

    protected override void OnDoubleTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        switch (obj.name)
        {
            case "Cube":
                {
                    // Find the local player object...
                    HoloLensPlayerScript localPlayer = new List<HoloLensPlayerScript>(GameObject.FindObjectsOfType<HoloLensPlayerScript>()).Find(player => player.isLocalPlayer);
                    if (localPlayer != null)
                    {
                        var was = localPlayer.gameObject.GetComponent<WorldAnchorScript>();
#if WINDOWS_UWP
                        WorldAnchorMgr.Instance.ImportWorldAnchorFromDisk();
#endif
                    }
                    break;
                }
        }
    }
}