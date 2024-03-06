using System.Collections.Generic;
using UnityEditor;

namespace YVR.Utilities
{
    public class YVRLogProjectSettingProvider : YVRProjectSettingProviderBase<YVRLogProjectSettingSO>
    {
        public YVRLogProjectSettingProvider(string path) : base(path, SettingsScope.Project) { }
        public override string settingsKey => "YVRLog";

        [SettingsProvider]
        public static SettingsProvider CreateYVRLogConfigSettings()
        {
            var provider = new YVRLogProjectSettingProvider("Project/YVR/YVRLog")
            {
                label = "YVR Log",
                keywords = new HashSet<string>(new[] {"YVR", "YVRLog"})
            };

            return provider;
        }
    }
}