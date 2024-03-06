using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSceneCaptureRequest
    {
        public int requestByteCount;

        [MarshalAs(UnmanagedType.LPStr)]
        public string request;
    }
}
