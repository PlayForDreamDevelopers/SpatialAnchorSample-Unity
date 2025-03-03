using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class LogController : MonoBehaviour
{
    public Text logContent;
    public ScrollRect debugInfoScrollRect;
    public static LogController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Log(string message, bool error = false)
    {
        if (logContent.text.Length>10000)
        {
            logContent.text = logContent.text.Substring(9000);
        }

        if (error)
        {
            logContent.text = $"{logContent.text}<color=#ff0000ff>{message}</color>"+"\n";
            Debug.LogError(message);
        }
        else
        {
            logContent.text =$"{logContent.text}{message}"+"\n";
            Debug.Log(message);
        }
        debugInfoScrollRect.verticalNormalizedPosition = 0.0f;
    }
}
