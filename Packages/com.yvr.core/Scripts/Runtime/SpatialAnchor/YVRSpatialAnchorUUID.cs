using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSpatialAnchorUUID
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRSpatialAnchor.k_SpaceUUIDLength)]
        public char[] Id;
    }
}