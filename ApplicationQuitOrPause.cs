using System;
using UnityEngine;

public class ApplicationQuitOrPause : MonoBehaviour
{
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private static Action quitActions;
    public static void Add(Action action)
    {
        quitActions += action;
    }
    public static void Remove(Action action)
    {
        quitActions -= action;
    }
    public static void RemoveAll()
    {
        foreach (Action quitAction in quitActions.GetInvocationList())
        {
            quitActions -= quitAction;
        }
    }

    private static void InvokeQuitActions()
    {
        if (quitActions != null)
        {
            quitActions.Invoke();
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            InvokeQuitActions();
        }
    }
#endif

#if UNITY_EDITOR || UNITY_IOS
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            InvokeQuitActions();
        }
    }

    private void OnApplicationQuit()
    {
        InvokeQuitActions();
    }
#endif
}
