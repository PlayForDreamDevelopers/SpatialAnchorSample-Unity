using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;

namespace YVR.Utilities.Test
{
    [Category("Log")]
    [TestFixture]
    public class YLogLoggerPlayerTests
    {
        private YLogLogger m_YLogLogger = null;
        private LoggerPriorityController m_PriorityController = null;

        [Test, Order(0)]
        public void CreateYLogLogger_NoneException()
        {
            Assert.That(() =>
            {
                m_YLogLogger = new YLogLogger();
                m_PriorityController = new LoggerPriorityController();

                YVRLog.SetLogger(m_YLogLogger);

                m_PriorityController.priority = LogPriority.Lowest;

                YLogLogger.ramLogPriorityTHold = LogPriority.Highest;
                YLogLogger.OutputViaUnity(true);
            }, Throws.Nothing);
        }

        [Test, Order(1)]
        public void Debug_LogcatWithHandler_TriggerUnityLog()
        {
            this.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, "Call Debug");
        }

        [Test, Order(2)]
        public void Info_LogcatWithHandler_TriggerUnityLog()
        {
            this.Info("Call Info");
            LogAssert.Expect(LogType.Log, "Call Info");
        }

        [Test, Order(3)]
        public void Warn_LogcatWithHandler_TriggerUnityWarn()
        {
            this.Warn("Call Warning");
            LogAssert.Expect(LogType.Warning, "Call Warning");
        }

        [Test, Order(4)]
        public void Error_LogcatWithHandler_TriggerUnityError()
        {
            this.Error("Call Error");
            LogAssert.Expect(LogType.Error, "Call Error");
        }

        [Test, Order(11)]
        public void SaveLog_Default_LogFilesIncreasedByOne()
        {
            YLogLogger.ConfigureYLog("YUnity");
            int originLogsCount = Application.isEditor ? 0 : Directory.GetFiles("/sdcard/misc/yvr/YUnity").Length;

            YLogLogger.SaveLog();

            int afterSaveLogsCount = Application.isEditor ? 1 : Directory.GetFiles("/sdcard/misc/yvr/YUnity").Length;

            Assert.That(afterSaveLogsCount - originLogsCount, Is.EqualTo(1));
        }
    }
}