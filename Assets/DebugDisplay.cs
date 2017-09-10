using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    TextMesh textMesh;

    // Use this for initialization
    void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMesh>();
    }

    void OnEnable()
    {
        Logger.log += LogMessage;
    }

    void OnDisable()
    {
        Logger.log -= LogMessage;
    }

    public void LogMessage(string message, LogType type)
    {
        if (textMesh.text.Length > 300)
        {
            textMesh.text = message + "\n";
        }
        else
        {
            textMesh.text += message + "\n";
        }
    }
}
