using UnityEngine;

public static class DLogger
{
    public static void Log(object message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
    public static void LogWarning(object message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
    }
    public static void LogError(object message)
    {
#if UNITY_EDITOR
        Debug.LogError(message);
#endif
    }

    public static void Log_Yellow(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=yellow>" + message + "</color>");
#endif
    }

    public static void Log_Red(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=red>" + message + "</color>");
#endif
    }

    public static void Log_Green(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=green>" + message + "</color>");
#endif
    }

    public static void Log_Blue(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=blue>" + message + "</color>");
#endif
    }
}
