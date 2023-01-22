using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuitOrPause : MonoBehaviour
{
    private static List<Action> quitActions = new List<Action>();
    public static void Add(Action action)
    {
        quitActions.Add(action);
    }

    private static void InvokeQuitActions()
    {
        foreach (var item in quitActions)
        {
            item.Invoke();
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
