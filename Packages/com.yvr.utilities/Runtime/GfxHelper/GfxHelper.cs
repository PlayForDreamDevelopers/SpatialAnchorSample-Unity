using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace YVR.Utilities
{
    public partial class GfxHelper : Singleton<GfxHelper>
    {
        private IGfxHelperHandler m_GfxHelperHandler = null;

        public GfxHelper()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        m_GfxHelperHandler = new GfxHelperAndroidHandler();
#else
            m_GfxHelperHandler = new GfxHelperEditorHandler();
#endif
        }

        public void TriggerGfxThreadCallback(Action callbackAction)
        {
            m_GfxHelperHandler.TriggerGfxThreadCallback(callbackAction);
        }

        public void TriggerCommandBufferThreadCalback(CommandBuffer commandBuffer, Action callback)
        {
            m_GfxHelperHandler.TriggerCommandBufferCallback(commandBuffer, callback);
        }

        public void CopyTexture(int srcTextureId, int dstTextureId)
        {
            m_GfxHelperHandler.CopyTexture(srcTextureId, dstTextureId);
        }

        public void CopyTexture(int srcTextureId, int dstTextureId, float alpha, Rect srcRect, Rect dstRect)
        {
            m_GfxHelperHandler.CopyTexture(srcTextureId, dstTextureId, alpha, srcRect, dstRect);
        }

        public void CopyTextureWithAlpha(int srcTextureId, int dstTextureId, float alpha)
        {
            m_GfxHelperHandler.CopyTextureWithAlpha(srcTextureId, dstTextureId, alpha);
        }

        public void CopyTextureRect(int srcTextureId, int dstTextureId, bool onlyShowAlpha, Rect rect)
        {
            m_GfxHelperHandler.CopyTextureRect(srcTextureId, dstTextureId, onlyShowAlpha, rect);
        }

        public void CopyAndroidTexture(int sourceTextureId, int destTextureId, IntPtr androidMutex)
        {
            m_GfxHelperHandler.CopyAndroidTexture(sourceTextureId, destTextureId, androidMutex);
        }

        public void CopyAndroidTexture(int srcTextureId, int dstTextureId, float alpha, bool isLinearSpace, Rect srcRect, Rect dstRect, IntPtr androidMutex)
        {
            m_GfxHelperHandler.CopyAndroidTexture(srcTextureId, dstTextureId, alpha, isLinearSpace, srcRect, dstRect, androidMutex);
        }

        public void CopyAndroidTextureWithAlpha(int sourceTextureId, int destTextureId, float alpha, bool isLinearSpace, IntPtr androidMutex)
        {
            m_GfxHelperHandler.CopyAndroidTextureWithAlpha(sourceTextureId, destTextureId, alpha, isLinearSpace, androidMutex);
        }

        public string GetTextureWritingFolderPath()
        {
            return m_GfxHelperHandler.GetTextureWritingFolderPath();
        }

        public void SetTextureWringFolderPath(string path)
        {
            m_GfxHelperHandler.SetTextureWritingFolderPath(path);
        }

        public void WriteTexture(int targetTextureId, string targetPath = "")
        {
            m_GfxHelperHandler.WriteTexture(targetTextureId, targetPath);
        }

        public void WriteAndroidTexture(int targetTextureId, int width, int height, IntPtr androidMutex, string targetPath = "")
        {
            m_GfxHelperHandler.WriteAndroidTexture(targetTextureId, width, height, androidMutex, targetPath);
        }

    }
}
