using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public class DebugManager : MonoBehaviorSingleton<DebugManager>
{
    [SerializeField] private int m_MaxLogLength = 10000;
    [SerializeField] private int m_MaxStackTraceLength = 10000;

    public readonly CompareInfo insensitiveComparer = new CultureInfo("en-US").CompareInfo;
    public StringBuilder sharedStringBuilder = new();

    public bool collapseLogs
    {
        get => m_CollapseLogs;
        set
        {
            m_CollapseLogs = value;
            ProcessLogData();
        }
    }

    public bool showInfo
    {
        get => m_ShowInfo;
        set
        {
            m_ShowInfo = value;
            ProcessLogData();
        }
    }

    public bool showWarning
    {
        get => m_ShowWarning;
        set
        {
            m_ShowWarning = value;
            ProcessLogData();
        }
    }

    public bool showError
    {
        get => m_ShowError;
        set
        {
            m_ShowError = value;
            ProcessLogData();
        }
    }

    public Action<List<DebugInfo>> onDebugInfosChanged;

    private bool m_CollapseLogs = false;
    private bool m_ShowInfo = true;
    private bool m_ShowWarning = true;
    private bool m_ShowError = true;
    private List<DebugInfo> m_DebugInfos = new();
    private List<DebugInfo> m_ProcessedInfos = new();
    private bool m_Initiated = false;
    private bool m_IsQuittingApplication = false;
    private float m_ElapsedSeconds;
    private int m_FrameCount;

    public void ClearDebug()
    {
        m_DebugInfos.Clear();
        ProcessLogData();
    }

    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);

        Application.logMessageReceivedThreaded += OnReceivedLog;
        Application.quitting += OnApplicationQuitting;

        m_Initiated = true;
    }

    private void OnDestroy()
    {
        if (!m_Initiated)
            return;

        Application.logMessageReceivedThreaded -= OnReceivedLog;
        Application.quitting -= OnApplicationQuitting;
    }

    private void Update()
    {
        m_ElapsedSeconds = Time.realtimeSinceStartup;
        m_FrameCount = Time.frameCount;
    }

    private void OnReceivedLog(string logString, string stackTrace, LogType logType)
    {
        if (m_IsQuittingApplication) return;

        logString = logString.Length > m_MaxLogLength
            ? logString.Substring(0, m_MaxLogLength - 12) + " <truncated>"
            : logString;

        stackTrace = stackTrace.Length > m_MaxStackTraceLength
            ? stackTrace.Substring(0, m_MaxStackTraceLength - 12) + " <truncated>"
            : stackTrace;

        m_DebugInfos.Add(new DebugInfo(logString, stackTrace, logType,
                                       new DebugInfoTimeStamp(DateTime.Now, m_ElapsedSeconds, m_FrameCount)));

        ProcessLogData();
    }

    private void ProcessLogData()
    {
        m_ProcessedInfos.Clear();

        foreach (DebugInfo debugInfo in m_DebugInfos)
        {
            debugInfo.ResetCount();

            if (!showInfo && debugInfo.logType == LogType.Log)
                continue;
            if (!showWarning && debugInfo.logType == LogType.Warning)
                continue;
            if (!showError && (debugInfo.logType == LogType.Error || debugInfo.logType == LogType.Exception))
                continue;

            if (collapseLogs)
            {
                DebugInfo existingInfo = m_ProcessedInfos.Find(info => info.Equals(debugInfo));
                if (existingInfo != null)
                    existingInfo.AddCount(1);
                else
                    m_ProcessedInfos.Add(debugInfo);
            }
            else
                m_ProcessedInfos.Add(debugInfo);
        }

        onDebugInfosChanged?.Invoke(m_ProcessedInfos);
    }

    private void OnApplicationQuitting() { m_IsQuittingApplication = true; }
}