using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSpatialAnchorListSaveInfo
    {
        public int anchorCount;
        public IntPtr anchorHandles;
        public YVRSpatialAnchorStorageLocation storageLocation;
    }
}