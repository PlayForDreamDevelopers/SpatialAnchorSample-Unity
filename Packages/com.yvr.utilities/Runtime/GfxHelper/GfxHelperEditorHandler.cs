using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace YVR.Utilities
{
    public partial class GfxHelperEditorHandler : IGfxHelperHandler
    {
        public void TriggerGfxThreadCallback(Action callback) { }
        public void TriggerCommandBufferCallback(CommandBuffer commandBuffer, Action callback) { }
        public void SetGfxThreadCallback() { }
        public void CopyTexture(int srcTextureId, int dstTextureId) { }
        public void CopyTexture(int srcTextureId, int dstTextureId, float alpha, Rect srcRect, Rect dstRect) { }
        public void CopyTextureWithAlpha(int srcTextureId, int dstTextureId, float alpha) { }
        public void CopyAndroidTexture(int sourceTextureId, int destTextureId, IntPtr androidMutex) { }
        public void CopyAndroidTexture(int srcTextureId, int dstTextureId, float alpha, bool isLinearSpace, Rect srcRect, Rect dstRect, IntPtr androidMutex) { }
        public void CopyAndroidTextureWithAlpha(int srcTextureId, int dstTextureId, float alpha, bool isLinearSpace, IntPtr androidMutex) { }
        public string GetTextureWritingFolderPath() { return ""; }
        public void SetTextureWritingFolderPath(string path) { }

        public void WriteTexture(int targetTextureId, string targetPath) { }

        public void WriteAndroidTexture(int targetTextureId, int width, int height, IntPtr androidMutex,
                                        string targetFilePath) { }

        public void CopyTextureRect(int srcTextureId, int dstTextureId, bool onlyShowAlpha, Rect viewportRect) { }
    }
}