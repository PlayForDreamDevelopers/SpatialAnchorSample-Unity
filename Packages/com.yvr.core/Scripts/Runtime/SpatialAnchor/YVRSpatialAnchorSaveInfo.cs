using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSpatialAnchorSaveInfo
    {
        public ulong anchorHandle;
        public YVRSpatialAnchorStorageLocation storageLocation;
    }
}