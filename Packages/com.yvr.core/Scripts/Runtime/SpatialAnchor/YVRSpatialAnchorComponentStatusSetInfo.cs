using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSpatialAnchorComponentStatusSetInfo
    {
        public YVRSpatialAnchorComponentType component;
        public bool enable;
        public double timeout;
    }
}