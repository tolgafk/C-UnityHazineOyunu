using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLog : MonoBehaviour
{
    public TMP_Text debugText;
    private string previousMessage = "";
    void Start()
    {
        debugText.text = "";
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            if (logString != previousMessage)
            {
                debugText.text += "\n" + logString;
                previousMessage = logString;
            }
        }
    }
}
