#if USE_YVR_TEST_FRAMRWORK
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Data;

namespace YVR.Utilities.Test
{
    [TestFixture, ConditionalIgnore("IgnoreForCoverage", "This is a performance test, not a unit test")]
    [Category("Performance")]
    [UnityPlatform(RuntimePlatform.Android)]
    public class LogcatPerformanceTests
    {
        [Test, Performance]
        [TestCase(LogPriority.Lowest, LogPriority.Highest)] // Only Logcat
        [TestCase(LogPriority.Highest, LogPriority.Lowest)] // Only RamLog
        [TestCase(LogPriority.Lowest, LogPriority.Lowest)] // Logcat And Ram Log
        [TestCase(LogPriority.Highest, LogPriority.Highest)] // Turn off logcat. In this case, also turn off unity log
        public void Debug_UnityVSAndroid(LogPriority logcat, LogPriority ramLog)
        {
            var priorityController = new LoggerPriorityController();
            var yLogLogger = new YLogLogger();
            priorityController.priority = logcat;

            yLogLogger.SetController(priorityController);
            YLogLogger.OutputViaUnity(false);
            YVRLog.SetLogger(yLogLogger);

            YLogLogger.ramLogPriorityTHold = ramLog;

            string androidLog = "Android Log";
            if (logcat == LogPriority.Lowest)
                androidLog += " with logcat";
            if (ramLog == LogPriority.Lowest)
                androidLog += " with ram log";

            Debug.unityLogger.logEnabled = !(logcat == LogPriority.Highest && ramLog == LogPriority.Highest);
            YVRLog.enable = !(logcat == LogPriority.Highest && ramLog == LogPriority.Highest);

            var unit = SampleUnit.Microsecond;
            var unityDebug = new MeasurementSettings(10, 100, 1, "Unity Debug.Log", unit);
            var androidDebug = new MeasurementSettings(10, 100, 1, androidLog, unit);

            int num = 0;
            Measure.Method(() =>
            {
                string output = $"Unity Number is {num++}";
                Debug.Log(output);
            }, unityDebug).Run();

            Measure.Method(() =>
            {
                string output = $"Android Number is {num++}";
                this.Debug(output);
            }, androidDebug).Run();
        }
    }
}
#endif