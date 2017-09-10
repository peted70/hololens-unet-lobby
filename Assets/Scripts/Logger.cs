using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    public delegate void LogCallback(string LogText, LogType type);
    public static event LogCallback log;

    public static void Log(string LogText, LogType type = LogType.Log)
    {
        if (log != null)
        {
            log(LogText, type);
        }
    }
}
