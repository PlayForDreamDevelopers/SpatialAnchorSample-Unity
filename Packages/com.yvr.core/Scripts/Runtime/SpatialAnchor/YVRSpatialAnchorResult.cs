using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{

    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSpatialAnchorResult
    {
        public ulong requestId;
        public ulong anchorHandle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRSpatialAnchor.k_SpaceUUIDLength)]
        public Char[] uuid;
    }
}