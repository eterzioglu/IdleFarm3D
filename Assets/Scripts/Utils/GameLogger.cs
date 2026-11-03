using System.Collections.Generic;
using UnityEngine;

public enum LogLevel { Info = 0, Warning = 1, Error = 2 }

public static class GameLogger
{
    public static bool Enabled = true;
    public static LogLevel MinLevel = LogLevel.Info;

    public static void Log(string message, LogLevel level = LogLevel.Info)
        => Write(level, message);

    private static void Write(LogLevel level, string message)
    {
        if (!Enabled) return;
        if (level < MinLevel) return;

        switch (level)
        {
            case LogLevel.Info:    Debug.Log(message); break;
            case LogLevel.Warning: Debug.LogWarning(message); break;
            case LogLevel.Error:   Debug.LogError(message); break;
        }
    }
}