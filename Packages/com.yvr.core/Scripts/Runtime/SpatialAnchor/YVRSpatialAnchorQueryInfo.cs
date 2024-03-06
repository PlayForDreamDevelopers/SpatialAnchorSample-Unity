using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSpatialAnchorQueryInfo
    {
        public uint MaxQuerySpaces;
        public double Timeout;
        public YVRSpatialAnchorStorageLocation storageLocation;
        public YVRSpatialAnchorComponentType component;
        public int numIds;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRSpatialAnchor.k_SpaceUuidMaxSize)]
        public YVRSpatialAnchorUUID[] ids;
    }
}