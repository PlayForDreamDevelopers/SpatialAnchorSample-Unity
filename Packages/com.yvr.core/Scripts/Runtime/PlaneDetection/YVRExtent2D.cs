using System.Runtime.InteropServices;

namespace YVR.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRExtent2D
    {
        public float width;
        public float height;
    }
}