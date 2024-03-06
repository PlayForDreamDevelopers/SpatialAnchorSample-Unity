using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRSpatialAnchorComponentStatus
    {
        public bool enable;
        public bool changePending;
    }
}