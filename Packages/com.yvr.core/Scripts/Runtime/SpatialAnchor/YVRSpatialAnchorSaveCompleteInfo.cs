using System.Runtime.InteropServices;

namespace YVR.Core
{
    public struct YVRSpatialAnchorSaveCompleteInfo
    {
        public ulong requestId;
        public int resultCode;
        public ulong anchorHandle;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRSpatialAnchor.k_SpaceUUIDLength)]
        public char[] uuid;

        public YVRSpatialAnchorStorageLocation location;
    }
}