using UnityEngine;

namespace YVR.Utilities
{
    public class YVRProjectSettingBaseSO<T> : ScriptableObject where T : YVRProjectSettingBaseSO<T>
    {
        private static T s_Instance = null;

        public static T instance
        {
            get
            {
                if (s_Instance != null) return s_Instance;
                YVRLog.Warn($"{typeof(T)} is null, should be set in project settings");
                s_Instance = CreateInstance<T>();

                return s_Instance;
            }
        }

        protected virtual void OnEnable() { s_Instance = (T) this; }
    }
}