using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.Utilities.Test
{
    [TestFixture, Category("Log")]
    public class UnityLoggerTests
    {
        private UnityLogger m_UnityLogger = null;
        private LoggerPriorityController m_PriorityController = null;

        [Test, Order(1)]
        public void CreateUnityLogger_NoException()
        {
            Assert.That(() =>
            {
                m_UnityLogger = new UnityLogger();
            }, Throws.Nothing);
        }

        [Test, Order(2)]
        public void SetLogger_UnityLogger_NoException()
        {
            Assert.That(() =>
            {
                YVRLog.SetLogger(m_UnityLogger);
            }, Throws.Nothing);
        }

        [Test, Order(3)]
        public void Debug_ReceiveDebug()
        {
            this.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, "Call Debug");
        }

        [Test, Order(4)]
        public void Info_ReceiveInfo()
        {
            this.Info("Call Info");
            LogAssert.Expect(LogType.Log, "Call Info");
        }

        [Test, Order(5)]
        public void Warn_ReceiveWarn()
        {
            this.Warn("Call Warn");
            LogAssert.Expect(LogType.Warning, "Call Warn");
        }

        [Test, Order(6)]
        public void Error_ReceiveError()
        {
            this.Error("Call Error");
            LogAssert.Expect(LogType.Error, "Call Error");
        }

        [Test, Order(7)]
        public void SetController_PriorityController_NoException()
        {
            Assert.That(() =>
            {
                m_PriorityController = new LoggerPriorityController();
                m_UnityLogger.SetController(m_PriorityController);
            }, Throws.Nothing);
        }

        [Test, Order(8)]
        public void SetPriorityTHold_Warn_DebugInfoNotOutput()
        {
            m_PriorityController.priority = LogPriority.Warn;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warn");
            this.Error("Call Error");

            LogAssert.Expect(LogType.Warning, "Call Warn");
            LogAssert.Expect(LogType.Error, "Call Error");
            LogAssert.NoUnexpectedReceived();
        }
    }
}