using HUX.Interaction;
using HUX.Receivers;
using UnityEngine;
using UnityEngine.VR.WSA;

public class PlayerReceiver : InteractionReceiver
{
    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        switch (obj.name)
        {
            case "Capsule":
                {
                    break;
                }
        }
    }
}
