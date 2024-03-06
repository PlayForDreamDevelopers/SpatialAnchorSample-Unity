using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRRect3D
    {
        public Vector3 offset;
        public Vector3 extent;
    }
}
