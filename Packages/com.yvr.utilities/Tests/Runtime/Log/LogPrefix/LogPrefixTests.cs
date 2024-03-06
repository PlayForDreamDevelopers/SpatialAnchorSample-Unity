using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.Utilities.Test
{
    [Category("Log")]
    [TestFixture]
    public class LogPrefixTests
    {
        private class ClassForTestPrefix { }

        private YLogLogger m_YLogLogger = null;
        private ClassForTestPrefix m_ClassForTestPrefix = null;

        private GameObject m_GOForTest = null;
        private MonoBehaviourForTest m_MonoForTest = null;
        private string m_StringForTest = null;

        private static string contextForClass => $"Context: {typeof(ClassForTestPrefix).FullName}";
        private static string priorityForDebug => $"Priority: {nameof(LogPriority.Debug)}";
        private string contextForMonoBehaviour => $"Context: {m_GOForTest.name}-{m_MonoForTest.GetType().FullName}";
        private string contextForGO => $"Context: {m_GOForTest.name}";
        private string contextForString => $"Context: {m_StringForTest}";

        [OneTimeSetUp]
        public void SetYLogLoggerPriority()
        {
            m_YLogLogger = new YLogLogger();
            m_ClassForTestPrefix = new ClassForTestPrefix();
            YVRLog.SetLogger(m_YLogLogger);
            YLogLogger.ramLogPriorityTHold = LogPriority.Highest;
            YLogLogger.OutputViaUnity(true);

            m_GOForTest = new GameObject("GameObjectForTest", typeof(MonoBehaviourForTest));
            m_MonoForTest = m_GOForTest.GetComponent<MonoBehaviourForTest>();
            m_StringForTest = "测试文本Aa1.";
        }

        [Test]
        public void Debug_ContextPrefixForClass_ClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix());
            m_ClassForTestPrefix.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[{contextForClass}] Call Debug");
        }

        [Test]
        public void Debug_PriorityPrefixForClass_PriorityInPrefix()
        {
            m_YLogLogger.SetPrefix(new PriorityLogPrefix());
            m_ClassForTestPrefix.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[{priorityForDebug}] Call Debug");
        }

        [Test]
        public void Debug_ContextPriorityPrefixForClass_ClassNamePriorityInPrefix()
        {
            m_YLogLogger.SetPrefix(new PriorityLogPrefix(new ContextLogPrefix()));
            m_ClassForTestPrefix.Debug("Call Debug");
            string exp = $"[{contextForClass} {priorityForDebug}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        [Test]
        public void Debug_PriorityContextPrefixForClass_PriorityClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix(new PriorityLogPrefix()));
            m_ClassForTestPrefix.Debug("Call Debug");
            string exp = $"[{priorityForDebug} {contextForClass}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        [Test]
        public void Debug_NonContext_NoException()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix(new PriorityLogPrefix()));
            YVRLog.Debug("[Priority: Debug NoneContext] Call Debug");
        }

        #region MonoBehavior

        [Test]
        public void Debug_ContextPrefixForMonobehaviour_ClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix());
            m_MonoForTest.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[{contextForMonoBehaviour}] Call Debug");
        }

        [Test]
        public void Debug_PriorityPrefixForMonobehaviour_PriorityInPrefix()
        {
            m_YLogLogger.SetPrefix(new PriorityLogPrefix());
            m_MonoForTest.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[Priority: {nameof(LogPriority.Debug)}] Call Debug");
        }

        [Test]
        public void Debug_ContextPriorityPrefixForMonobehaviour_ClassNamePriorityInPrefix()
        {
            m_YLogLogger.SetPrefix(new PriorityLogPrefix(new ContextLogPrefix()));
            m_MonoForTest.Debug("Call Debug");
            string exp = $"[{contextForMonoBehaviour} {priorityForDebug}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        [Test]
        public void Debug_PriorityContextPrefixForMonobehaviour_PriorityClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix(new PriorityLogPrefix()));
            m_MonoForTest.Debug("Call Debug");
            string exp = $"[{priorityForDebug} {contextForMonoBehaviour}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        #endregion

        #region GameObject

        [Test]
        public void Debug_ContextPrefixForGO_ClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix());
            m_GOForTest.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[{contextForGO}] Call Debug");
        }

        [Test]
        public void Debug_PriorityPrefixForGO_PriorityInPrefix()
        {
            m_YLogLogger.SetPrefix(new PriorityLogPrefix());
            m_GOForTest.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[Priority: {nameof(LogPriority.Debug)}] Call Debug");
        }

        [Test]
        public void Debug_ContextPriorityPrefixForGO_ClassNamePriorityInPrefix()
        {
            m_YLogLogger.SetPrefix(new PriorityLogPrefix(new ContextLogPrefix()));
            m_GOForTest.Debug("Call Debug");
            string exp = $"[{contextForGO} {priorityForDebug}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        [Test]
        public void Debug_PriorityContextPrefixForGO_PriorityClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix(new PriorityLogPrefix()));
            m_GOForTest.Debug("Call Debug");
            string exp = $"[{priorityForDebug} {contextForGO}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        [Test]
        public void Debug_CustomPriorityContextPrefixForGO_PriorityClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix(new PriorityLogPrefix(new CustomLogPrefix("YVR"))));
            m_GOForTest.Debug("Call Debug");
            string exp = $"[YVR {priorityForDebug} {contextForGO}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        #endregion

        #region String

        [Test]
        public void Debug_ContextPrefixForString_ClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix());
            m_StringForTest.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[{contextForString}] Call Debug");
        }

        [Test]
        public void Debug_PriorityPrefixForString_PriorityInPrefix()
        {
            m_YLogLogger.SetPrefix(new PriorityLogPrefix());
            m_StringForTest.Debug("Call Debug");
            LogAssert.Expect(LogType.Log, $"[Priority: {nameof(LogPriority.Debug)}] Call Debug");
        }

        [Test]
        public void Debug_ContextPriorityPrefixForString_ClassNamePriorityInPrefix()
        {
            m_YLogLogger.SetPrefix(new PriorityLogPrefix(new ContextLogPrefix()));
            m_StringForTest.Debug("Call Debug");
            string exp = $"[{contextForString} {priorityForDebug}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        [Test]
        public void Debug_PriorityContextPrefixForString_PriorityClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix(new PriorityLogPrefix()));
            m_StringForTest.Debug("Call Debug");
            string exp = $"[{priorityForDebug} {contextForString}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        [Test]
        public void Debug_CustomPriorityContextPrefixForString_PriorityClassNameInPrefix()
        {
            m_YLogLogger.SetPrefix(new ContextLogPrefix(new PriorityLogPrefix(new CustomLogPrefix("YVR"))));
            m_StringForTest.Debug("Call Debug");
            string exp = $"[YVR {priorityForDebug} {contextForString}] Call Debug";
            LogAssert.Expect(LogType.Log, exp);
        }

        #endregion
    }
}