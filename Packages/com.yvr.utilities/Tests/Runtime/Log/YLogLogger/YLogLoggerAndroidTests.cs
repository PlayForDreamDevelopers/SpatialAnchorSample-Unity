using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.Utilities.Test
{
    [TestFixture, Category("Log"), UnityPlatform(RuntimePlatform.Android)]
    public class YLogLoggerAndroidTests
    {
        private YLogLogger m_YLogLogger = new();
        private LoggerPriorityController m_PriorityController = new();

        [OneTimeSetUp]
        public void SetYLogLoggerPriorityHighest()
        {
            m_PriorityController.priority = LogPriority.Lowest;
            m_YLogLogger.SetController(m_PriorityController);

            YVRLog.SetLogger(m_YLogLogger);
            YLogLogger.ramLogPriorityTHold = LogPriority.Highest;

            YLogLogger.OutputViaUnity(true);
        }

        [UnityTest, Order(12)]
        public IEnumerator Debug_RamLog_GetDebugFromLogFile()
        {
            YLogLogger.ramLogPriorityTHold = LogPriority.Lowest;
            m_PriorityController.priority = LogPriority.Highest;

            this.Debug("Call Debug");

            yield return new WaitForSecondsRealtime(1.0f);
            YLogLogger.SaveLog();

            string latestFilePath = GetLatestLogFilePath();
            string lastLine = GetFileLastSeveralLines(latestFilePath, 1)[0];

            Assert.That(lastLine.Contains("Call Debug"), "lastLine.Contains('Call Debug')");
            Assert.That(lastLine.Contains("[DEBUG]"), "lastLine.Contains('[Debug]')");
        }

        [UnityTest, Order(13)]
        public IEnumerator Info_RamLog_GetInfoFromLogFile()
        {
            this.Info("Call Info");

            YLogLogger.SaveLog();
            yield return new WaitForSecondsRealtime(1.0f);

            string latestFilePath = GetLatestLogFilePath();
            string lastLine = GetFileLastSeveralLines(latestFilePath, 1)[0];
            Assert.That(lastLine.Contains("Call Info"), "lastLine.Contains('Call Info')");
            Assert.That(lastLine.Contains("[INFO]"), "lastLine.Contains('[INFO]')");
        }

        [UnityTest, Order(14)]
        public IEnumerator Warn_RamLog_GetWarnFromLogFile()
        {
            this.Warn("Call Warning");

            YLogLogger.SaveLog();
            yield return new WaitForSecondsRealtime(1.0f);

            string latestFilePath = GetLatestLogFilePath();
            string lastLine = GetFileLastSeveralLines(latestFilePath, 1)[0];
            Assert.That(lastLine.Contains("Call Warning"), "lastLine.Contains('Call Warning')");
            Assert.That(lastLine.Contains("[WARN]"), "lastLine.Contains('[WARN]')");
        }

        [UnityTest, Order(15)]
        public IEnumerator Error_RamLog_GetErrorFromLogFile()
        {
            this.Error("Call Error");

            YLogLogger.SaveLog();
            yield return new WaitForSecondsRealtime(1.0f);

            string latestFilePath = GetLatestLogFilePath();
            string lastLine = GetFileLastSeveralLines(latestFilePath, 1)[0];
            Assert.That(lastLine.Contains("Call Error"), "lastLine.Contains('Call Error')");
            Assert.That(lastLine.Contains("[ERROR]"), "lastLine.Contains('[ERROR]')");
        }

        [UnityTest, Order(16)]
        public IEnumerator SetRamLogPriority_Debug_AllOutput()
        {
            YLogLogger.ramLogPriorityTHold = LogPriority.Debug;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            string latestFilePath = GetLatestLogFilePath();
            int previousLength = GetFileLinesCount(latestFilePath);

            YLogLogger.SaveLog();
            yield return new WaitForSecondsRealtime(1.0f);

            latestFilePath = GetLatestLogFilePath();
            int currentLinesCount = GetFileLinesCount(latestFilePath);
            string[] lastFourLines = GetFileLastSeveralLines(latestFilePath, 4);

            Assert.That(currentLinesCount - previousLength, Is.EqualTo(4));
            Assert.That(lastFourLines[0].Contains("Call Debug"));
            Assert.That(lastFourLines[1].Contains("Call Info"));
            Assert.That(lastFourLines[2].Contains("Call Warning"));
            Assert.That(lastFourLines[3].Contains("Call Error"));
        }

        [UnityTest, Order(17)]
        public IEnumerator SetRamLogPriority_Info_InfoWarnErrorOutput()
        {
            YLogLogger.ramLogPriorityTHold = LogPriority.Info;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            string latestFilePath = GetLatestLogFilePath();
            int previousLength = GetFileLinesCount(latestFilePath);

            YLogLogger.SaveLog();
            yield return new WaitForSecondsRealtime(1.0f);

            latestFilePath = GetLatestLogFilePath();
            int currentLinesCount = GetFileLinesCount(latestFilePath);
            string[] lastFourLines = GetFileLastSeveralLines(latestFilePath, 3);

            Assert.That(currentLinesCount - previousLength, Is.EqualTo(3));
            Assert.That(lastFourLines[0].Contains("Call Info"));
            Assert.That(lastFourLines[1].Contains("Call Warning"));
            Assert.That(lastFourLines[2].Contains("Call Error"));
        }

        [UnityTest, Order(18)]
        public IEnumerator SetRamLogPriority_Warn_WarnErrorOutput()
        {
            YLogLogger.ramLogPriorityTHold = LogPriority.Warn;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            string latestFilePath = GetLatestLogFilePath();
            int previousLength = GetFileLinesCount(latestFilePath);

            YLogLogger.SaveLog();
            yield return new WaitForSecondsRealtime(1.0f);

            latestFilePath = GetLatestLogFilePath();
            int currentLinesCount = GetFileLinesCount(latestFilePath);
            string[] lastFourLines = GetFileLastSeveralLines(latestFilePath, 2);

            Assert.That(currentLinesCount - previousLength, Is.EqualTo(2));
            Assert.That(lastFourLines[0].Contains("Call Warning"));
            Assert.That(lastFourLines[1].Contains("Call Error"));
        }

        [UnityTest, Order(19)]
        public IEnumerator SetRamLogPriority_Error_ErrorOutput()
        {
            YLogLogger.ramLogPriorityTHold = LogPriority.Error;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            string latestFilePath = GetLatestLogFilePath();
            int previousLength = GetFileLinesCount(latestFilePath);

            YLogLogger.SaveLog();
            yield return new WaitForSecondsRealtime(1.0f);

            latestFilePath = GetLatestLogFilePath();
            int currentLinesCount = GetFileLinesCount(latestFilePath);
            string[] lastFourLines = GetFileLastSeveralLines(latestFilePath, 1);

            Assert.That(currentLinesCount - previousLength, Is.EqualTo(1));
            Assert.That(lastFourLines[0].Contains("Call Error"));
        }

        [UnityTest, Order(20)]
        public IEnumerator SetRamLogPriority_Highest_NoOutput()
        {
            YLogLogger.ramLogPriorityTHold = LogPriority.Highest;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            string latestFilePath = GetLatestLogFilePath();
            int previousLength = GetFileLinesCount(latestFilePath);

            YLogLogger.SaveLog();
            yield return new WaitForSecondsRealtime(1.0f);

            latestFilePath = GetLatestLogFilePath();
            int currentLinesCount = GetFileLinesCount(latestFilePath);

            Assert.That(currentLinesCount, Is.EqualTo(previousLength));
        }

        private int GetFileLinesCount(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return 0;
            return File.ReadLines(filePath).ToArray().Length;
        }

        private string GetLatestLogFilePath(string dir = "/sdcard/misc/yvr/YUnity")
        {
            List<string> orderedFiles = Directory.GetFiles(dir)
                                                 .OrderByDescending(filePath => new FileInfo(filePath).CreationTime)
                                                 .ToList();
            if (!orderedFiles.Any()) return "";

            string latestFilePath = orderedFiles[0];

            return latestFilePath;
        }

        private string[] GetFileLastSeveralLines(string filePath, int targetLines)
        {
            Debug.Log($"file Path is {filePath}");
            string[] allLines = File.ReadLines(filePath).ToArray();

            int totalLength = allLines.Length;
            if (totalLength == 0 || totalLength < targetLines) return null;

            string[] lastLines = new string[targetLines];

            for (int line = 0; line < targetLines; line++)
                lastLines[line] = allLines[totalLength - targetLines + line];

            return lastLines;
        }
    }
}