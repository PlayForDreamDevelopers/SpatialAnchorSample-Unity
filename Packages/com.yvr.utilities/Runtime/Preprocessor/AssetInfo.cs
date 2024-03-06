using System;

[Serializable]
public class AssetInfo
{
    /// <summary>
    /// should delete this file in destination path?
    /// </summary>
    public bool shouldDelete;

    /// <summary>
    /// asset file source path
    /// </summary>
    public string unityAssetPath;

    /// <summary>
    /// asset file destination path
    /// </summary>
    public string androidProjectAssetPath;
}
