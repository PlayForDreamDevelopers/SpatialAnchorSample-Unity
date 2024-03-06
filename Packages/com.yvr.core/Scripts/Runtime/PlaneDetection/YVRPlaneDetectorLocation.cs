using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRPlaneDetectorLocation
    {
        public ulong planeId;
        public YVRSpaceLocationFlags locationFlags;
        public PoseData poseData;
        public YVRExtent2D yvrExtents;
        public YVRPlaneDetectorOrientation orientation;
        public YVRPlaneDetectorSemanticType semanticType;
        public uint polygonBufferCount;
        public IntPtr polygonBufferIntPtr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0)]
        public YVRPlaneDetectorPolygonBuffer[] polygonBuffers;
    }
}