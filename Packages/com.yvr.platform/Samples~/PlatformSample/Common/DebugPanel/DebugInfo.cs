using System.Globalization;
using System.Text;
using UnityEngine;

public class DebugInfo
{
    public LogType logType { private set; get; }
    public string logString { private set; get; }
    public string stackTrace { private set; get; }
    public int collapseCount { private set; get; }

    public DebugInfoTimeStamp timeStamp { private set; get; }

    private string m_CompleteLog;

    public DebugInfo(string logString, string stackTrace, LogType logType, DebugInfoTimeStamp timeStamp)
    {
        this.logString = logString;
        this.stackTrace = stackTrace;
        this.logType = logType;
        this.timeStamp = timeStamp;

        m_CompleteLog = null;
        collapseCount = 1;
    }

    public bool Equals(DebugInfo other)
    {
        return logType == other.logType && logString == other.logString && stackTrace == other.stackTrace;
    }

    public bool MatchesSearchTerm(string searchTerm)
    {
        return (logString != null &&
                DebugManager.instance.insensitiveComparer.IndexOf(logString, searchTerm,
                                                                  CompareOptions.IgnoreCase |
                                                                  CompareOptions.IgnoreNonSpace) >=
                0) ||
               (stackTrace != null &&
                DebugManager.instance.insensitiveComparer.IndexOf(stackTrace, searchTerm,
                                                                  CompareOptions.IgnoreCase |
                                                                  CompareOptions.IgnoreNonSpace) >=
                0);
    }

    public void AddCount(int delta) { collapseCount += delta; }
    public void ResetCount() { collapseCount = 1; }

    public override string ToString() { return m_CompleteLog ??= string.Concat(logString, "\n", stackTrace); }
}

public class DebugInfoTimeStamp
{
    public readonly System.DateTime dateTime;
    public readonly float elapsedSeconds;
    public readonly int frameCount;

    public DebugInfoTimeStamp(System.DateTime dateTime, float elapsedSeconds, int frameCount)
    {
        this.dateTime = dateTime;
        this.elapsedSeconds = elapsedSeconds;
        this.frameCount = frameCount;
    }

    public void AppendTime(StringBuilder sb)
    {
        sb.Append("[");

        int hour = dateTime.Hour;
        if (hour >= 10)
            sb.Append(hour);
        else
            sb.Append("0").Append(hour);

        sb.Append(":");

        int minute = dateTime.Minute;
        if (minute >= 10)
            sb.Append(minute);
        else
            sb.Append("0").Append(minute);

        sb.Append(":");

        int second = dateTime.Second;
        if (second >= 10)
            sb.Append(second);
        else
            sb.Append("0").Append(second);

        sb.Append("]");
    }

    public void AppendFullTimestamp(StringBuilder sb)
    {
        AppendTime(sb);
        sb.Append("[").Append(elapsedSeconds.ToString("F1")).Append("s at ").Append("#").Append(frameCount).Append("]");
    }
}