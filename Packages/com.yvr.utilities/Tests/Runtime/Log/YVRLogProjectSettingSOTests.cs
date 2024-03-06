#if USE_YVR_TEST_FRAMRWORK
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using YVR.Test.Framework;
using Object = UnityEngine.Object;

namespace YVR.Utilities.Test
{
    [TestFixture, Category("Log")]
    public class YVRLogProjectSettingSOTests : IPrebuildSetup, IPostBuildCleanup
    {
        private class MyClass1 { }

        private class MyClass2 { }

        private string m_FolderPath = "Packages/com.yvr.utilities/Tests/Runtime/Log/Config";
        private string scenePath => $"{m_FolderPath}/YVRLogConfigTests.unity";
        public void Setup() { TestUtils.AddRequiredScene(scenePath); }
        public void Cleanup() { TestUtils.RemoveRequiredScene(scenePath); }

        private YVRLogProjectSettingSO setting => YVRLogProjectSettingSO.instance;

        private void ReInit(YVRLogConfigSO config)
        {
            setting.configSO = config;

            MethodInfo initMethod
                = typeof(YVRLogProjectSettingSO).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic);
            initMethod.Invoke(setting, null);
        }

        [UnityTest]
        public IEnumerator _00_LoadSceneSucceed()
        {
            yield return TestUtils.TestScene(scenePath, () =>
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                Debug.Log("Current Scene: " + currentSceneName);
                Assert.That(currentSceneName.Contains("YVRLogConfigTests"));
            });
        }

        [Test]
        public void _01_EmptyConfig_NoException()
        {
            Assert.That(() =>
            {
                ReInit(null);
            }, Throws.Nothing);
        }

        [Test]
        public void _02_ValidLocalConfig_PreferLocal_UseLocalConfig()
        {
            string jsonPath = Path.GetFullPath($"{m_FolderPath}/YVRLogConfig.json");
            string localName = JsonConvert.DeserializeObject<YVRLogConfigSO>(File.ReadAllText(jsonPath)).name;
            YVRLogProjectSettingSO.instance.localConfigPath = jsonPath;
            YVRLogProjectSettingSO.instance.preferLocalConfig = true;

            // This Config should be override by local config
            var config = ScriptableObject.CreateInstance<YVRLogConfigSO>();
            config.name = "SoConfig";
            ReInit(config);

            Assert.That(YVRLogProjectSettingSO.instance.configSO.name == localName,
                        "Config Name is not from local config");
        }

        [Test]
        public void _03_ValidLocalConfig_NotPreferLocal_UseSOConfig()
        {
            string jsonPath = Path.GetFullPath($"{m_FolderPath}/YVRLogConfig.json");
            setting.localConfigPath = jsonPath;
            setting.preferLocalConfig = false;

            // This Config should be override by local config
            var config = ScriptableObject.CreateInstance<YVRLogConfigSO>();
            config.name = "SoConfig";
            ReInit(config);

            Assert.That(setting.configSO.name == config.name, "Config Name is not from local config");
        }

        [Test]
        public void _05_InvalidLocalConfig_UseSOConfig()
        {
            string jsonPath = Path.GetFullPath($"{m_FolderPath}/InvalidYVRLogConfig.json");
            setting.localConfigPath = jsonPath;
            setting.preferLocalConfig = true;

            // This Config should be override by local config
            var config = ScriptableObject.CreateInstance<YVRLogConfigSO>();
            config.name = "SoConfig";
            ReInit(config);
            LogAssert.Expect(LogType.Error, new Regex("Load local log configuration .* failed."));

            Assert.That(setting.configSO.name == config.name, "Config Name is not from so config");
            setting.localConfigPath = "";
        }

        [Test]
        public void _10_Config_NoException()
        {
            var config = ScriptableObject.CreateInstance<YVRLogConfigSO>();
            Assert.That(() =>
            {
                ReInit(config);
            }, Throws.Nothing);
            Object.Destroy(config);
        }

        [Test]
        public void _11_DisableConfig_NoOutput()
        {
            var config = ScriptableObject.CreateInstance<YVRLogConfigSO>();
            config.enable = false;

           setting .ConfigYVRLog(config);

            this.Debug("Debug");
            this.Info("Info");
            this.Warn("Info");
            this.Error("Error");
            LogAssert.NoUnexpectedReceived();

            Object.Destroy(config);
        }

        [Test]
        public void _12_OnlyUnityLogConfig_AllOutput()
        {
            var config = ScriptableObject.CreateInstance<YVRLogConfigSO>();
            config.enable = true;
            config.loggerConfigs = new List<LoggerConfig>()
            {
                new() {loggerType = LoggerType.Unity}
            };

            setting.ConfigYVRLog(config);

            this.Debug("Debug");
            this.Info("Info");
            this.Warn("Warning");
            this.Error("Error");
            LogAssert.Expect(LogType.Log, "Debug");
            LogAssert.Expect(LogType.Log, "Info");
            LogAssert.Expect(LogType.Warning, "Warning");
            LogAssert.Expect(LogType.Error, "Error");

            Object.Destroy(config);
        }

        [Test]
        public void _13_UnityLogConfig_NoController_WarnPriority_GetWarningButStillFiltering()
        {
            var config = ScriptableObject.CreateInstance<YVRLogConfigSO>();
            config.enable = true;
            config.loggerConfigs = new List<LoggerConfig>()
            {
                new() {loggerType = LoggerType.Unity, priority = LogPriority.Warn}
            };

            setting.ConfigYVRLog(config);

            this.Debug("Debug");
            this.Info("Info");
            this.Warn("Warning");
            this.Error("Error");
            LogAssert.Expect(LogType.Warning, "Warning");
            LogAssert.Expect(LogType.Error, "Error");
            LogAssert.NoUnexpectedReceived();

            Object.Destroy(config);
        }

        [Test]
        public void _14_UnityLogConfig_PriorityPrefix_GetPrefix()
        {
            var config = ScriptableObject.CreateInstance<YVRLogConfigSO>();
            config.enable = true;
            config.loggerConfigs = new List<LoggerConfig>()
            {
                new()
                    {loggerType = LoggerType.Unity, prefixType = PrefixType.Priority}
            };

            setting.ConfigYVRLog(config);

            this.Debug("Debug");
            string priorityForDebug = $"Priority: {nameof(LogPriority.Debug)}";

            LogAssert.Expect(LogType.Log, $"[{priorityForDebug}] Debug");
            Object.Destroy(config);
        }

        [Test]
        public void _15_UnityLogConfig_Prefix2Priority_PrefixPriorityControllerWork()
        {
            var config = ScriptableObject.CreateInstance<YVRLogConfigSO>();
            config.enable = true;
            config.loggerConfigs = new List<LoggerConfig>()
            {
                new()
                {
                    loggerType = LoggerType.Unity, prefixType = PrefixType.Context, priority = LogPriority.Highest,
                    prefix2PriorityMap = new List<Prefix2PriorityNode>()
                    {
                        new("Class1", LogPriority.Info),
                        new("Class2", LogPriority.Warn),
                    }
                },
            };

            setting.ConfigYVRLog(config);

            var myClass1 = new MyClass1();
            var myClass2 = new MyClass2();

            this.Debug("This Debug");
            this.Info("This Info");
            this.Warn("This Warn");
            this.Error("This Error");

            myClass1.Debug("MyClass1 Debug");
            myClass1.Info("MyClass1 Info");
            myClass1.Warn("MyClass1 Warn");
            myClass1.Error("MyClass1 Error");

            myClass2.Debug("MyClass2 Debug");
            myClass2.Info("MyClass2 Info");
            myClass2.Warn("MyClass2 Warn");
            myClass2.Error("MyClass2 Error");

            LogAssert.Expect(LogType.Log, $"[Context: {myClass1.GetType().FullName}] MyClass1 Info");
            LogAssert.Expect(LogType.Warning, $"[Context: {myClass1.GetType().FullName}] MyClass1 Warn");
            LogAssert.Expect(LogType.Error, $"[Context: {myClass1.GetType().FullName}] MyClass1 Error");

            LogAssert.Expect(LogType.Warning, $"[Context: {myClass2.GetType().FullName}] MyClass2 Warn");
            LogAssert.Expect(LogType.Error, $"[Context: {myClass2.GetType().FullName}] MyClass2 Error");

            Object.Destroy(config);
        }
    }
}
#endif