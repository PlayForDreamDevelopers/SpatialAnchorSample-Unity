using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRPlaneDetectorLocations
    {
        public uint planeLocationCountOutput;
        public IntPtr planeLocationsIntPtr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0)]
        public YVRPlaneDetectorLocation[] planeLocation;
    }
}