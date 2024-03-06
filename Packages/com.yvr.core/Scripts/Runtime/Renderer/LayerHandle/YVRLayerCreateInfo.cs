using System;
using System.Runtime.InteropServices;

namespace YVR.Core
{
    [Serializable]
    public enum YVRRenderLayerEyeMask
    {
        kEyeMaskLeft = 0x00000001,
        kEyeMaskRight = 0x00000002,
        kEyeMaskBoth = 0x00000003
    }

    [Serializable]
    public enum YVRRenderLayerType
    {
        Projection = 0,
        Quad,
        Cylinder,
        Equirect,
        Equirect2
    }

    public struct YVRLayerCreateInfo
    {
        public int depth;
        public int width;
        public int height;
        public int swapChainBufferCount;
        public int textureType;
        public int layerFlags;
        [MarshalAs(UnmanagedType.U1)] public bool separateSwapChain;
        [MarshalAs(UnmanagedType.U1)] public bool autoShowAfterCreated;
        public YVRRenderLayerType renderLayerType;
        public YVRRenderLayerEyeMask renderLayerEyeMask;

        public YVRLayerCreateInfo(int depth, int width, int height, int swapChainBufferCount,
                                  YVRRenderLayerType renderLayerType, YVRRenderLayerEyeMask renderLayerEyeMask = YVRRenderLayerEyeMask.kEyeMaskBoth, bool autoShowAfterCreated = true)
            : this(depth, width, height, swapChainBufferCount, 0, 2, false, autoShowAfterCreated, renderLayerType, renderLayerEyeMask) { }

        public YVRLayerCreateInfo(int depth, int width, int height, int swapChainBufferCount, int textureType,
                                  int layerFlags, bool separateSwapChain, bool autoShowAfterCreated, YVRRenderLayerType renderLayerType, YVRRenderLayerEyeMask renderLayerEyeMask)
        {
            this.depth = depth;
            this.width = width;
            this.height = height;
            this.swapChainBufferCount = swapChainBufferCount;
            this.textureType = textureType;
            this.layerFlags = layerFlags;
            this.separateSwapChain = separateSwapChain;
            this.autoShowAfterCreated = autoShowAfterCreated;
            this.renderLayerType = renderLayerType;
            this.renderLayerEyeMask = renderLayerEyeMask;
        }
    }
}