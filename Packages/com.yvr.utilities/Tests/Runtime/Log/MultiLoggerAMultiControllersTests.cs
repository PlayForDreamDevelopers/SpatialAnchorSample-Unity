using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.Utilities.Test
{
    [TestFixture, Category("Log")]
    public class MultiLoggerAMultiControllersTests
    {
        private class ClassForTest { }

        private class Class2ForTest { }

        private YLogLogger m_YLogLogger = null;
        private UnityLogger m_UnityLogger = null;
        private CustomLogPrefix m_CustomLogPrefix4YLog = null;
        private CustomLogPrefix m_CustomLogPrefix4YUnity = null;
        private ContextLogPrefix m_ContextCustomPrefix4YLog = null;
        private ContextLogPrefix m_ContextCustomPrefix4Unity = null;
        private LoggerPrefixPriorityController m_YLoggerPrefixPriorityController = null;
        private LoggerPrefixPriorityController m_UnityPrefixPriorityController = null;

        private ClassForTest m_ClassForTest = new();
        private Class2ForTest m_Class2ForTest = new();

        [Test, Order(1)]
        public void _01_CreateTestRequiredLoggersAControllersAPrefix_NoException()
        {
            Assert.That(() =>
            {
                m_YLogLogger = new YLogLogger();
                m_UnityLogger = new UnityLogger();
                m_YLoggerPrefixPriorityController = new LoggerPrefixPriorityController();
                m_UnityPrefixPriorityController = new LoggerPrefixPriorityController();
                m_CustomLogPrefix4YLog = new CustomLogPrefix("Prefix 4 YLog");
                m_CustomLogPrefix4YUnity = new CustomLogPrefix("Prefix 4 Unity");

                m_ContextCustomPrefix4YLog = new ContextLogPrefix(m_CustomLogPrefix4YLog);
                m_ContextCustomPrefix4Unity = new ContextLogPrefix(m_CustomLogPrefix4YUnity);

                m_YLogLogger.SetController(m_YLoggerPrefixPriorityController);
                m_UnityLogger.SetController(m_UnityPrefixPriorityController);

                m_YLogLogger.SetPrefix(m_CustomLogPrefix4YLog);
                m_UnityLogger.SetPrefix(m_CustomLogPrefix4YUnity);

                YVRLog.ClearLoggers();
                YVRLog.RegisterLogger(m_YLogLogger);
                YVRLog.RegisterLogger(m_UnityLogger);
            }, Throws.Nothing);
        }

        [Test, Order(2)]
        public void _02_Debug_ReceiveTwoDebug()
        {
            this.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, "[Prefix 4 YLog] Call Debug");
            LogAssert.Expect(LogType.Log, "[Prefix 4 Unity] Call Debug");
        }

        [Test, Order(3)]
        public void _03_Debug_YLogPriorityHighest_UnityPriorityLowest_OnlyReceiveUnityLog()
        {
            m_YLoggerPrefixPriorityController.priority = LogPriority.Highest;
            m_UnityPrefixPriorityController.priority = LogPriority.Lowest;
            this.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, "[Prefix 4 Unity] Call Debug");
        }

        [Test, Order(4)]
        public void _04_Debug_YLogPriorityLowest_UnityPriorityHighest_OnlyReceiveYLogLog()
        {
            m_YLoggerPrefixPriorityController.priority = LogPriority.Lowest;
            m_UnityPrefixPriorityController.priority = LogPriority.Highest;
            this.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, "[Prefix 4 YLog] Call Debug");
        }

        [Test, Order(5)]
        public void _05_SetContextCustomPrefix_ReceiveTargetYLogLog()
        {
            m_YLoggerPrefixPriorityController.priority = LogPriority.Lowest;
            m_UnityPrefixPriorityController.priority = LogPriority.Highest;

            m_YLogLogger.SetPrefix(m_ContextCustomPrefix4YLog);
            m_UnityLogger.SetPrefix(m_ContextCustomPrefix4Unity);

            this.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[Prefix 4 YLog Context: {GetType().FullName}] Call Debug");

            m_ClassForTest.Debug("Debug for class");
            LogAssert.Expect(LogType.Log, $"[Prefix 4 YLog Context: {m_ClassForTest.GetType().FullName}] Debug for class");
        }

        [Test, Order(6)]
        public void _06_SetMyClassContextLowestPriority4UnityLogger_ReceiveYLogLog()
        {
            m_YLoggerPrefixPriorityController.priority = LogPriority.Lowest;
            m_UnityPrefixPriorityController.priority = LogPriority.Highest;

            m_UnityPrefixPriorityController.AddPrefix2PriorityMap(nameof(ClassForTest), LogPriority.Lowest);

            m_YLogLogger.SetPrefix(m_ContextCustomPrefix4YLog);
            m_UnityLogger.SetPrefix(m_ContextCustomPrefix4Unity);

            this.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[Prefix 4 YLog Context: {GetType().FullName}] Call Debug");

            m_ClassForTest.Debug("Debug for class");
            LogAssert.Expect(LogType.Log, $"[Prefix 4 YLog Context: {m_ClassForTest.GetType().FullName}] Debug for class");
            LogAssert.Expect(LogType.Log, $"[Prefix 4 Unity Context: {m_ClassForTest.GetType().FullName}] Debug for class");

            m_Class2ForTest.Debug("Debug for class2");
            LogAssert.Expect(LogType.Log, $"[Prefix 4 YLog Context: {m_Class2ForTest.GetType().FullName}] Debug for class2");
        }
    }
}



