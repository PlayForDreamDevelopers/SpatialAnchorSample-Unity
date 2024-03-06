using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRQuerySpatialAnchorResult
    {
        public uint numResults;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRSpatialAnchor.k_SpaceUuidMaxSize)]
        public YVRSpatialAnchorResult[] results;
    }
}