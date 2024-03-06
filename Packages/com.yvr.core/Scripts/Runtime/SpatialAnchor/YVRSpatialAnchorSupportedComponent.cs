using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSpatialAnchorSupportedComponent
    {
        public int numComponents;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = YVRSpatialAnchor.k_SpaceFilterInfoComponentsMaxSize)]
        public YVRSpatialAnchorComponentType[] components;
    }
}