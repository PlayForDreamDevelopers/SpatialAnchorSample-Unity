using System.Runtime.InteropServices;
using UnityEngine;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRRect2D
    {
        public Vector2 offset;
        public Vector2 extent;
    }
}