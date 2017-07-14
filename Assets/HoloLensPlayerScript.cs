using UnityEngine;
using UnityEngine.Networking;

public class HoloLensPlayerScript : NetworkBehaviour
{
    [SyncVar]
    public Vector3 position;

    [SyncVar]
    public Quaternion rotation;

    public override void OnStartClient()
    {
        CmdPositionRotationChanged(position, rotation);
        base.OnStartClient();
    }

    [Command]
    private void CmdPositionRotationChanged(Vector3 inpos, Quaternion inrot)
    {
        position = inpos;
        rotation = inrot;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            // probably want to set our own position as well..
            CmdPositionRotationChanged(Camera.main.transform.position, Camera.main.transform.rotation);
        }
        else
        {
            transform.position = position;
            transform.localRotation = rotation;
        }
    }
}
