using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.Utilities.Test
{
    [TestFixture, Category("Log")]
    public class LogPrefixPriorityControllerTests
    {
        private class ClassForTestPrefix { }

        private YLogLogger m_YLogLogger = new();
        private LoggerPrefixPriorityController m_ControllerForTest = new();
        private ContextLogPrefix m_ContextPrefix = new();

        private ClassForTestPrefix m_ClassForTestPrefix = new();
        private GameObject m_GOForTest = null;
        private MonoBehaviourForTest m_MonoForTest = null;

        [OneTimeSetUp]
        public void SetYLogLoggerPriorityHighest()
        {
            m_ControllerForTest.priority = LogPriority.Lowest;
            m_YLogLogger.SetController(m_ControllerForTest);
            m_YLogLogger.SetPrefix(m_ContextPrefix);

            YVRLog.SetLogger(m_YLogLogger);
            YLogLogger.OutputViaUnity(true);

            m_GOForTest = new GameObject("GameObjectForTest", typeof(MonoBehaviourForTest));
            m_MonoForTest = m_GOForTest.GetComponent<MonoBehaviourForTest>();
        }

        #region TestsAboutDict

        [Test]
        public void AddPrefix2PriorityMap_DuplicateElements_NoException()
        {
            var controllerForTest = new LoggerPrefixPriorityController();

            Assert.That(() =>
            {
                controllerForTest.AddPrefix2PriorityMap("ABC", LogPriority.Debug);
                controllerForTest.AddPrefix2PriorityMap("ABC", LogPriority.Debug);
            }, Throws.Nothing);
        }

        [Test]
        public void AddPrefix2PriorityMap_SamePrefixDifferentPriority_OverridePriority()
        {
            var controllerForTest = new LoggerPrefixPriorityController();
            var priority = LogPriority.Lowest;

            Assert.That(() =>
            {
                controllerForTest.AddPrefix2PriorityMap("ABC", LogPriority.Debug);
                controllerForTest.AddPrefix2PriorityMap("ABC", LogPriority.Error);

                controllerForTest.TryGetFieldValue("m_Prefix2PriorityDic", out Dictionary<string, LogPriority> dict);
                dict.TryGetValue("ABC", out priority);
            }, Throws.Nothing);

            Assert.That(priority, Is.EqualTo(LogPriority.Error));
        }

        [Test]
        public void RemovePrefixPriorityMap_RemoveNonExistElement_NoException()
        {
            var controllerForTest = new LoggerPrefixPriorityController();

            Assert.That(() =>
            {
                controllerForTest.RemovePrefixPriorityMap("ABC");
            }, Throws.Nothing);
        }

        [Test]
        public void RemovePrefixPriorityMap_RemoveExistElement_RemoveSuccess()
        {
            var controllerForTest = new LoggerPrefixPriorityController();
            var priority = LogPriority.Lowest;

            Assert.That(() =>
            {
                controllerForTest.AddPrefix2PriorityMap("ABC", LogPriority.Debug);
                controllerForTest.RemovePrefixPriorityMap("ABC");

                controllerForTest.TryGetFieldValue("m_Prefix2PriorityDic", out Dictionary<string, LogPriority> dict);
                dict.TryGetValue("ABC", out priority);
            }, Throws.Nothing);

            Assert.That(priority, Is.EqualTo(LogPriority.Lowest));
        }

        [Test]
        public void ClearPrefixPriority_ClearEmptyDic_NoException()
        {
            var controllerForTest = new LoggerPrefixPriorityController();

            Assert.That(() =>
            {
                controllerForTest.ClearPrefixPriority();
            }, Throws.Nothing);
        }

        [Test]
        public void ClearPrefixPriority_ClearNonEmptyDic_ClearSuccess()
        {
            var controllerForTest = new LoggerPrefixPriorityController();
            var priority = LogPriority.Lowest;

            Assert.That(() =>
            {
                controllerForTest.AddPrefix2PriorityMap("ABC", LogPriority.Debug);
                controllerForTest.ClearPrefixPriority();

                controllerForTest.TryGetFieldValue("m_Prefix2PriorityDic", out Dictionary<string, LogPriority> dict);
                dict.TryGetValue("ABC", out priority);
            }, Throws.Nothing);

            Assert.That(priority, Is.EqualTo(LogPriority.Lowest));
        }

        #endregion

        #region TestsAboutPriorityFilter

        [Test]
        public void Priority_GlobalPriorityLowest_AllOutput()
        {
            m_ControllerForTest.priority = LogPriority.Lowest;
            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            LogAssert.Expect(LogType.Log, new Regex(".* Call Debug"));
            LogAssert.Expect(LogType.Log, new Regex(".* Call Info"));
            LogAssert.Expect(LogType.Warning, new Regex(".* Call Warning"));
            LogAssert.Expect(LogType.Error, new Regex(".* Call Error"));
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void Priority_GlobalPriorityHighest_NothingOutput()
        {
            m_ControllerForTest.priority = LogPriority.Highest;
            this.Debug("Call Debug");
            this.Info("Call Info");
            this.Warn("Call Warning");
            this.Error("Call Error");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void Debug_PriorityHighest_PrefixLowest_TargetClass_Output()
        {
            m_ControllerForTest.priority = LogPriority.Highest;
            m_ControllerForTest.AddPrefix2PriorityMap(nameof(ClassForTestPrefix), LogPriority.Lowest);
            m_ClassForTestPrefix.Debug("Call Debug");

            LogAssert.Expect(LogType.Log, new Regex(@".* Call Debug"));
        }

        [Test]
        public void Debug_PriorityHighest_PrefixLowest_NotTargetClass_Output()
        {
            m_ControllerForTest.priority = LogPriority.Highest;
            m_ControllerForTest.AddPrefix2PriorityMap(nameof(ClassForTestPrefix), LogPriority.Lowest);
            this.Debug("Call Debug");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void Debug_PriorityHighest_PrefixLowest_TargetMono_Output()
        {
            m_ControllerForTest.priority = LogPriority.Highest;
            m_ControllerForTest.AddPrefix2PriorityMap(nameof(MonoBehaviourForTest), LogPriority.Lowest);
            m_MonoForTest.Debug("Call Debug");

            LogAssert.Expect(LogType.Log, new Regex(@".* Call Debug"));
        }

        [Test]
        public void Debug_PriorityHighest_PrefixLowest_NotTargetMono_Output()
        {
            m_ControllerForTest.priority = LogPriority.Highest;
            m_ControllerForTest.AddPrefix2PriorityMap(nameof(ClassForTestPrefix), LogPriority.Lowest);
            this.Debug("Call Debug");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void Debug_PriorityHighest_PrefixLowest_TargetGO_Output()
        {
            m_ControllerForTest.priority = LogPriority.Highest;
            m_ControllerForTest.AddPrefix2PriorityMap(m_GOForTest.name, LogPriority.Lowest);
            m_GOForTest.Debug("Call Debug");

            LogAssert.Expect(LogType.Log, new Regex(@".* Call Debug"));
        }

        [Test]
        public void Debug_PriorityHighest_PrefixLowest_NotTargetGO_Output()
        {
            m_ControllerForTest.priority = LogPriority.Highest;
            m_ControllerForTest.AddPrefix2PriorityMap(m_GOForTest.name, LogPriority.Lowest);

            var abc = new GameObject("abc");
            abc.Debug("Call Debug");
            
            LogAssert.NoUnexpectedReceived();
        }

        #endregion
    }
}