using UnityEngine;

namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of Entitlement
    /// </summary>
    public class Entitlement
    {
        /// <summary>
        /// Is user entitled to this app
        /// </summary>
        public readonly bool isEntitled;

        public Entitlement(AndroidJavaObject obj) { isEntitled = YVRPlatform.YVRPermissionIsViewerEntitled(obj); }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append($"isEntitled:[{isEntitled}],\n\r");

            return str.ToString();
        }
    }
}