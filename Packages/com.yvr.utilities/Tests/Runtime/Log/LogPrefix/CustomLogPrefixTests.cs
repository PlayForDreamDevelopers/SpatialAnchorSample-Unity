using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.Utilities.Test
{
    [TestFixture, Category("Log")]
    public class CustomLogPrefixTests
    {
        private CustomLogPrefix m_CustomLogPrefix4YLog = null;
        private CustomLogPrefix m_CustomLogPrefix4Unity = null;
        private YLogLogger m_YLogLogger = null;
        private UnityLogger m_UnityLogger = null;

        [Test, Order(1)]
        public void _01_CreateCustomLogPrefix_SetToYLogLogger_NoException()
        {
            Assert.That(() =>
            {
                m_CustomLogPrefix4YLog = new CustomLogPrefix("Custom Prefix 4 YLog");
                m_YLogLogger = new YLogLogger();
                YLogLogger.OutputViaUnity(true);
                m_YLogLogger.SetPrefix(m_CustomLogPrefix4YLog);
            }, Throws.Nothing);
        }

        [Test, Order(2)]
        public void _02_CreateCustomLogPrefix_SetToUnityLogger_NoException()
        {
            Assert.That(() =>
            {
                m_CustomLogPrefix4Unity = new CustomLogPrefix("Custom Prefix 4 Unity");
                m_UnityLogger = new UnityLogger();
                m_UnityLogger.SetPrefix(m_CustomLogPrefix4Unity);
            }, Throws.Nothing);
        }

        [Test, Order(3)]
        public void _03_SetLogger_YLogLoggerAUnityLogger_NoException()
        {
            Assert.That(() =>
            {
                YVRLog.SetLogger(m_YLogLogger);
                YVRLog.RegisterLogger(m_UnityLogger);
            }, Throws.Nothing);
        }

        [Test, Order(4)]
        public void _04_Debug_ReceiveTwoDebug_WithTargetPrefix()
        {
            this.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, "[Custom Prefix 4 YLog] Call Debug");
            LogAssert.Expect(LogType.Log, "[Custom Prefix 4 Unity] Call Debug");
        }

        [Test, Order(5)]
        public void _05_Info_ChangePrefix_WithTargetPrefix()
        {
            m_CustomLogPrefix4YLog.prefix = "Custom Prefix 1";
            this.Info("Call Info");
            LogAssert.Expect(LogType.Log, "[Custom Prefix 1] Call Info");
            LogAssert.Expect(LogType.Log, "[Custom Prefix 4 Unity] Call Info");
        }
    }
}