using System.Runtime.InteropServices;

namespace YVR.Core
{
    public struct YVRSpatialAnchorSetStatusCompleteInfo
    {
        public ulong requestId;
        public int resultCode;

        public ulong anchorHandle;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRSpatialAnchor.k_SpaceUUIDLength)]
        public char[] uuid;
        public YVRSpatialAnchorComponentType componentType;
        public bool enabled;
    }
}