using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HoloLensPlayerScript : NetworkBehaviour
{
    [SyncVar(hook ="OnId")]
    public string id;

    [Command]
    public void CmdIdChanged(string name)
    {
        id = name;
    }

    public void OnId(string newId)
    {
        id = newId;
        var label = gameObject.transform.Find("Label");
        var tm = label.GetComponent<TextMesh>();
        tm.text = newId;
    }
}
