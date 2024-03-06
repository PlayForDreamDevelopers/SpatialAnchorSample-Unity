using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRRoomLayout
    {
        public YVRSpatialAnchorUUID floorUuid;

        public YVRSpatialAnchorUUID ceilingUuid;

        public int wallUuidCapacityInput;

        public int wallUuidCountOutput;

        public IntPtr wallUuidsPtr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0)]
        public YVRSpatialAnchorUUID[] wallUuids;
    }
}