using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSpatialAnchorShareInfo
    {
        public int anchorCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRSpatialAnchor.k_SpaceUuidMaxSize)]
        public ulong[] anchorHandle;

        public int userCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRSpatialAnchor.k_ShareUserMaxSize)]
        public ulong[] users;
    }
}