using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YVR.Utilities
{
    public class YVRProjectSettingsMapSO : ScriptableObject
    {
        [SerializeField] private List<Key2SettingPathNode> key2ConfigPathMap = new();

        private const string k_YVRProjectSettingsSOPath = "Assets/YVRProjectSettings/YVRProjectSettingsMap.asset";

        public static YVRProjectSettingsMapSO GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<YVRProjectSettingsMapSO>(k_YVRProjectSettingsSOPath);
            if (settings != null) return settings;
            if (!AssetDatabase.IsValidFolder("Assets/YVRProjectSettings"))
                AssetDatabase.CreateFolder("Assets", "YVRProjectSettings");
            settings = CreateInstance<YVRProjectSettingsMapSO>();
            AssetDatabase.CreateAsset(settings, k_YVRProjectSettingsSOPath);
            AssetDatabase.SaveAssets();

            return settings;
        }

        public void AddConfig(string key, string configPath)
        {
            key2ConfigPathMap.Add(new Key2SettingPathNode(key, configPath));
            SaveSettings();
        }

        public void RemoveConfig(string key)
        {
            Key2SettingPathNode node = key2ConfigPathMap.Find(n => n.key == key);
            if (node == null) return;

            key2ConfigPathMap.Remove(node);
            SaveSettings();
        }

        public string GetConfig(string key)
        {
            Key2SettingPathNode node = key2ConfigPathMap.Find(n => n.key == key);
            return node?.path;
        }

        private void SaveSettings()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}