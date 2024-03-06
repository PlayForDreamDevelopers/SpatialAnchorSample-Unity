using System.Runtime.InteropServices;

namespace YVR.Core.XR
{
    [StructLayout(LayoutKind.Sequential)]
    public struct YVRXRUserDefinedSettings
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool use16BitDepthBuffer;

        [MarshalAs(UnmanagedType.U1)]
        public bool useMonoscopic;

        [MarshalAs(UnmanagedType.U1)]
        public bool useLinearColorSpace;

        [MarshalAs(UnmanagedType.U1)]
        public bool UseVRWidget;

        [MarshalAs(UnmanagedType.U1)]
        public bool useAppSW;

        [MarshalAs(UnmanagedType.U1)]
        public bool optimizeBufferDiscards;

        public ushort stereoRenderingMode;
        public float eyeRenderScale;
    }
}