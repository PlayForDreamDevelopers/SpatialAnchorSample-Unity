using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace YVR.Utilities
{
    public abstract class YVRProjectSettingProviderBase<T> : SettingsProvider where T : ScriptableObject
    {
        public abstract string settingsKey { get; }
        protected T config;
        protected static YVRProjectSettingsMapSO projectSettingsMap = null;

        protected YVRProjectSettingProviderBase(string path, SettingsScope scopes) : base(path, scopes) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            projectSettingsMap = YVRProjectSettingsMapSO.GetOrCreateSettings();

            string localStorageConfigPath = projectSettingsMap.GetConfig(settingsKey);

            config = string.IsNullOrEmpty(localStorageConfigPath)
                ? null
                : AssetDatabase.LoadAssetAtPath<T>(projectSettingsMap.GetConfig(settingsKey));
        }

        public override void OnGUI(string searchCOntext)
        {
            Object obj = EditorGUILayout.ObjectField("Config", config, typeof(T), false);
            var newConfig = obj as T;
            if (newConfig == config) return;

            var preloadAssets = new List<Object>(PlayerSettings.GetPreloadedAssets());
            if (config != null)
            {
                preloadAssets.Remove(config);
                projectSettingsMap.RemoveConfig(settingsKey);
            }

            if (newConfig != null)
            {
                preloadAssets.Add(newConfig);
                projectSettingsMap.AddConfig(settingsKey, AssetDatabase.GetAssetPath(newConfig));
            }

            PlayerSettings.SetPreloadedAssets(preloadAssets.ToArray());
            config = newConfig;
        }
    }
}