using System;

namespace YVR.Utilities
{
    [Serializable]
    public class Key2SettingPathNode
    {
        public string key;
        public string path;

        public Key2SettingPathNode(string key, string path)
        {
            this.key = key;
            this.path = path;
        }
    }
}