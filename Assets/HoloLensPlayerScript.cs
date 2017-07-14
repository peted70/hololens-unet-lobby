using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class HoloLensPlayerScript : NetworkBehaviour
{
    [SyncVar]
    public Vector3 position;

    [SyncVar]
    public Quaternion rotation;
    private Color _playerColour;

    public Color PlayerColour
    {
        get { return _playerColour; }
        set
        {
            if (_playerColour != value)
            {
                _playerColour = value;
                SetDeviceColour(_playerColour);
            }
        }
    }

    private void SetDeviceColour(Color playerColour)
    {
        // change the tint colour of the HoloLens models visor
        var deviceGo = gameObject.GetComponentsInChildren<Transform>();
        var device = deviceGo.Where(t => t.name == "hololens").Single() as Transform;
        var mr = device.GetComponents<MeshRenderer>();
        foreach (var mat in mr[0].materials)
        {
            if (mat.name.Contains("glass"))
            {
                mat.color = playerColour;
                break;
            }
        }
    }

    public string PlayerName { get; internal set; }

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
