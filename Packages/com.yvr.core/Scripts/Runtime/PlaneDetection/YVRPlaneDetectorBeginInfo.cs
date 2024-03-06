using System.Runtime.InteropServices;
using System.Security.Permissions;
using UnityEngine;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRPlaneDetectorBeginInfo
    {
        public int orientationCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRPlaneDetectorMgr.k_PlaneDetectorOrientationCount)]
        public YVRPlaneDetectorOrientation[] orientations;
        public int semanticTypeCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRPlaneDetectorMgr.k_SemanticTypeCount)]
        public YVRPlaneDetectorSemanticType[] semanticTypes;
        public int maxPlanes;
        public float minArea;
        public PoseData boundingBoxPose;
        public Vector3 boundingBoxExtent;
    }
}