using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRPlaneDetectorPolygonBuffer
    {
        public uint vertexCountOutput;
        public IntPtr verticesIntPtr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0)]
        public Vector2[] point;
    }
}