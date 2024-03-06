using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRBoundary2D
    {
        public int vertexCapacityInput;
        public int vertexCountOutput;
        public IntPtr vertices;
    }
}