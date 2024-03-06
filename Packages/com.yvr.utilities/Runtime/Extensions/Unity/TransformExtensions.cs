using UnityEngine;

namespace YVR.Utilities
{
    public static class TransformExtensions
    {
        private static Vector3 s_CacheVec3;

        public static Transform LocalPositionZ(this Transform trans, float z) 
        {
            s_CacheVec3 = trans.localPosition;
            s_CacheVec3.z = z;
            trans.localPosition = s_CacheVec3;
            return trans;
        }
        
        public static Transform LocalPositionY(this Transform trans, float y)
        {
            s_CacheVec3 = trans.localPosition;
            s_CacheVec3.y = y;
            trans.localPosition = s_CacheVec3;
            return trans;
        }
        
        public static Transform PositionY(this Transform trans, float y)
        {
            s_CacheVec3 = trans.position;
            s_CacheVec3.y = y;
            trans.position = s_CacheVec3;
            return trans;
        }
        
        public static Transform LocalIdentity(this Transform trans)
        {
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
            return trans;
        }
    }
}