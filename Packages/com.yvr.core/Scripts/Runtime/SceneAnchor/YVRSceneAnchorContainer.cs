using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSceneAnchorContainer
    {
        public int uuidCapacityInput;
        public int uuidCountOutput;
        public IntPtr uuids;
    }
}