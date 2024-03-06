using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.Utilities.Test
{
    [Category("Log")]
    public class YVRLogTests
    {
        private UnityLogger m_UnityLogger = new();

        [Test, Order(0)]
        public void Output_NoneContext_NoException()
        {
            YVRLog.enable = true;
            YVRLog.SetLogger(new YLogLogger());

            YVRLog.Debug("Call Debug");
            YVRLog.Info("Call Info");
            YVRLog.Warn("Call Warn");
            YVRLog.Error("Call Error");

            LogAssert.Expect(LogType.Log, "Call Debug");
            LogAssert.Expect(LogType.Log, "Call Info");
            LogAssert.Expect(LogType.Warning, "Call Warn");
            LogAssert.Expect(LogType.Error, "Call Error");
        }

        [Test]
        public void ClearLoggers_DefaultDebug_NoUnityDebug()
        {
            YVRLog.ClearLoggers();
            this.Debug("Call Debug");
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void RegisterLogger_TwoDefaultLogger_TriggerTwice()
        {
            YVRLog.ClearLoggers();
            YVRLog.RegisterLogger(m_UnityLogger);
            YVRLog.RegisterLogger(m_UnityLogger);

            this.Debug("Call Debug");

            LogAssert.Expect(LogType.Log, "Call Debug");
            LogAssert.Expect(LogType.Log, "Call Debug");
        }

        [Test]
        public void UnRegisterLogger_UnRegisterOneFromTwoDefaultLogger_TriggerOnce()
        {
            // Firstly register two default loggers
            YVRLog.ClearLoggers();
            YVRLog.RegisterLogger(m_UnityLogger);
            YVRLog.RegisterLogger(m_UnityLogger);

            // Unregister one default logger
            YVRLog.UnregisterLogger(m_UnityLogger);

            this.Debug("Call Debug");

            LogAssert.Expect(LogType.Log, "Call Debug");
        }

        [Test]
        public void Enable_False_NoUnityDebug()
        {
            YVRLog.ClearLoggers();
            YVRLog.enable = false;
            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Info");
            this.Error("Call Error");

            LogAssert.NoUnexpectedReceived();

            YVRLog.enable = true;
        }
    }
}