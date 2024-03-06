using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRAnchorSemanticLabel
    {
        public int byteCapacityInput;
        public int byteCountOutput;
        public IntPtr labels;
    }
}