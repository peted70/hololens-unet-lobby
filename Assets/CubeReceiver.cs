using HUX.Interaction;
using HUX.Receivers;
using UnityEngine;
using UnityEngine.VR.WSA;

public class CubeReceiver : InteractionReceiver
{
    protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
    {
        switch (obj.name)
        {
            case "Cube":
                {
                    var sharedCollection = Targets[0];
                    var was = sharedCollection.GetComponent<WorldAnchorScript>();
                    var wa = was.GetComponent<WorldAnchor>();
                    if (wa == null)
                        wa = was.gameObject.AddComponent<WorldAnchor>();

                    was.UploadWorldAnchor(wa);
                    break;
                }
        }
    }
}