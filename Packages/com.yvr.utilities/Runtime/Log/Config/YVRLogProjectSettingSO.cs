using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace YVR.Utilities
{
    [CreateAssetMenu(fileName = "YVRLogProjectSetting", menuName = "YVR/ProjectSettings/YVRLog")]
    public class YVRLogProjectSettingSO : YVRProjectSettingBaseSO<YVRLogProjectSettingSO>
    {
        public YVRLogConfigSO configSO;
        public string localConfigPath = "";
        public bool preferLocalConfig = true;

        protected override void OnEnable()
        {
            base.OnEnable();
             
            configSO = preferLocalConfig ? (LoadConfigViaLocalFile() ?? configSO) : configSO;

            ConfigYVRLog(configSO);
            return;

            YVRLogConfigSO LoadConfigViaLocalFile()
            {
                if (string.IsNullOrEmpty(localConfigPath) || !File.Exists(localConfigPath)) return null;

                try
                {
                    return JsonConvert.DeserializeObject<YVRLogConfigSO>(File.ReadAllText(localConfigPath));
                } catch
                {
                    Debug.LogError($"Load local log configuration {localConfigPath} failed.");
                    return null;
                }
            }
        }
        
        public void ConfigYVRLog(YVRLogConfigSO configSO = null)
        {
            this.configSO = configSO ? configSO : this.configSO;
            this.configSO?.Apply();
        }
    }
}