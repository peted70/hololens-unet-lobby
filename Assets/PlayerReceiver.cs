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
            case "Player(Clone)":
                {
                    //var ps = obj.GetComponent<HoloLensPlayerScript>();
                    //ps.CmdIdChanged("id " + Random.value);
                    break;
                }
        }
        base.OnTapped(obj, eventArgs);
    }
}
