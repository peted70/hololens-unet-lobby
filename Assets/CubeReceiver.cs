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
                    HoloLensPlayerScript localPlayer = new List<HoloLensPlayerScript>(GameObject.FindObjectsOfType<HoloLensPlayerScript>()).Find(player => player.isLocalPlayer);
                    if (localPlayer != null)
                    {
                        var was = localPlayer.gameObject.GetComponent<WorldAnchorScript>();
                        var wa = obj.GetComponent<WorldAnchor>();
                        if (wa == null)
                            wa = obj.AddComponent<WorldAnchor>();

                        was.UploadWorldAnchor(wa);
                    }
                    break;
                }
        }
    }
}