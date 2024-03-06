using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace YVR.Utilities
{
    public partial interface IGfxHelperHandler
    {
        void TriggerGfxThreadCallback(Action callback);
        void TriggerCommandBufferCallback(CommandBuffer commandBuffer, Action callback);
        void SetGfxThreadCallback();
        void CopyTexture(int srcTextureId, int dstTextureId);
        void CopyTexture(int srcTextureId, int dstTextureId, float alpha, Rect srcRect, Rect dstRect);
        void CopyTextureWithAlpha(int srcTextureId, int dstTextureId, float alpha);
        void CopyTextureRect(int srcTextureId, int dstTextureId, bool onlyShowAlpha, Rect viewportRect);
        void CopyAndroidTexture(int sourceTextureId, int destTextureId, IntPtr androidMutex);
        void CopyAndroidTexture(int srcTextureId, int dstTextureId, float alpha, bool isLinearSpace, Rect srcRect, Rect dstRect, IntPtr androidMutex);
        void CopyAndroidTextureWithAlpha(int srcTextureId, int dstTextureId, float alpha, bool isLinearSpace, IntPtr androidMutex);
        string GetTextureWritingFolderPath();
        void SetTextureWritingFolderPath(string path);

        void WriteTexture(int targetTextureId, string targetPath);
        void WriteAndroidTexture(int targetTextureId, int width, int height, IntPtr androidMutex, string targetFilePath);
    }
}