using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.Utilities.Test
{
    [Category("Log")]
    [TestFixture]
    public class LogPriorityControllerTests
    {
        private YLogLogger m_YLogLogger = new();
        private LoggerPriorityController m_PriorityController = new();

        [OneTimeSetUp]
        public void SetYLogLoggerPriorityHighest()
        {
            m_PriorityController.priority = LogPriority.Lowest;
            m_YLogLogger.SetController(m_PriorityController);

            YVRLog.SetLogger(m_YLogLogger);

            YLogLogger.OutputViaUnity(true);
        }

        [Test, Order(1)]
        public void Priority_Lowest_AllOutput()
        {
            YVRLog.SetLogger(m_YLogLogger);
            m_PriorityController.priority = LogPriority.Lowest;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            LogAssert.Expect(LogType.Log, "Call Debug");
            LogAssert.Expect(LogType.Log, "Call Info");
            LogAssert.Expect(LogType.Warning, "Call Warning");
            LogAssert.Expect(LogType.Error, "Call Error");

            LogAssert.NoUnexpectedReceived();
        }

        [Test, Order(2)]
        public void Priority_Debug_AllOutput()
        {
            m_PriorityController.priority = LogPriority.Debug;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            LogAssert.Expect(LogType.Log, "Call Debug");
            LogAssert.Expect(LogType.Log, "Call Info");
            LogAssert.Expect(LogType.Warning, "Call Warning");
            LogAssert.Expect(LogType.Error, "Call Error");
            LogAssert.NoUnexpectedReceived();
        }

        [Test, Order(3)]
        public void Priority_Info_InfoWarnErrorOutput()
        {
            m_PriorityController.priority = LogPriority.Info;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            LogAssert.Expect(LogType.Log, "Call Info");
            LogAssert.Expect(LogType.Warning, "Call Warning");
            LogAssert.Expect(LogType.Error, "Call Error");
            LogAssert.NoUnexpectedReceived();
        }

        [Test, Order(4)]
        public void Priority_Warn_WarnErrorOutput()
        {
            m_PriorityController.priority = LogPriority.Warn;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            LogAssert.Expect(LogType.Warning, "Call Warning");
            LogAssert.Expect(LogType.Error, "Call Error");
            LogAssert.NoUnexpectedReceived();
        }

        [Test, Order(5)]
        public void Priority_Error_OnlyError()
        {
            m_PriorityController.priority = LogPriority.Error;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            LogAssert.Expect(LogType.Error, "Call Error");
            LogAssert.NoUnexpectedReceived();
        }

        [Test, Order(6)]
        public void Priority_Highest_NoAnyOutput()
        {
            m_PriorityController.priority = LogPriority.Highest;

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            LogAssert.NoUnexpectedReceived();
        }

        [Test, Order(7)]
        public void SetController_None_AllOutput()
        {
            m_YLogLogger.SetController(null);

            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            LogAssert.Expect(LogType.Log, "Call Debug");
            LogAssert.Expect(LogType.Log, "Call Info");
            LogAssert.Expect(LogType.Warning, "Call Warning");
            LogAssert.Expect(LogType.Error, "Call Error");
            LogAssert.NoUnexpectedReceived();
        }
    }
}