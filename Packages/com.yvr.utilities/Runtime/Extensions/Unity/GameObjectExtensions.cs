using UnityEngine;

namespace YVR.Utilities
{
    public static class GameObjectExtensions
    {
        public static GameObject Show(this GameObject selfObj)
        {
            selfObj.SetActive(true);
            return selfObj;
        }
        
        public static GameObject Hide(this GameObject selfObj)
        {
            selfObj.SetActive(false);
            return selfObj;
        }
    }
}