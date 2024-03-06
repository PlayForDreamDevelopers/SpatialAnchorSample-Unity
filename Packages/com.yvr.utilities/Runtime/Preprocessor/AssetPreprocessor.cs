using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AssetPreprocessor
{
    public static void CopyAsset(List<AssetInfo> assetInfoList)
    {
#if UNITY_EDITOR
        assetInfoList.ForEach(assetInfo =>
        {
            if (assetInfo.shouldDelete)
            {
                FileUtil.DeleteFileOrDirectory(assetInfo.androidProjectAssetPath);
            }
            else
            {
                FileUtil.ReplaceFile(assetInfo.unityAssetPath, assetInfo.androidProjectAssetPath);
            }
        });
#endif
    }
}
