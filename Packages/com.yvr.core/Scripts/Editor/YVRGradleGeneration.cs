using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using System.IO;
using AndroidManifest;
using UnityEditor.Android;
using UnityEngine.Internal;
using UnityEditor.VersionControl;
using System.Linq;
using System;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Threading;

namespace YVR.Core.XR
{
    [ExcludeFromDocs]
    [InitializeOnLoad]
    public class YvrGradleGeneration : IPostGenerateGradleAndroidProject
    {
        private const string ToolName = "YVR/Tools/";
        private const string MenuItemVROnly = ToolName + "VR mode";
        private const string MenuItem6Dof = ToolName + "Only 6Dof";
        private static bool _sixDof = true;
        private static bool _vrOnly;
        private const string SaveDataPath = "Assets/XR/Resources/";
        private static readonly string SettingAssetPath = $"{SaveDataPath}{typeof(YVRSDKSettingAsset)}.asset";

        private static readonly string AssetFilePath =
            $"{Application.dataPath}/XR/Resources/{typeof(YVRSDKSettingAsset)}.asset";

        static YvrGradleGeneration()
        {
            EditorApplication.delayCall += OnDelayCall;
        }

        [MenuItem(MenuItemVROnly)]
        private static void ToggleVROnly()
        {
            _vrOnly = !_vrOnly;

            Menu.SetChecked(MenuItemVROnly, _vrOnly);

            GetVrOnlyTagInfo(out YVRSDKSettingAsset asset, _vrOnly).required = _vrOnly;
            GetImmerseHMDTagInfo(out asset).required = _vrOnly;

            var sourceFile = $"Assets/Plugins/Android/AndroidManifest.xml";
            if (File.Exists(sourceFile))
                ManifestPreprocessor.PatchAndroidManifest(asset.manifestTagInfosList, sourceFile);


            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("select vr only: " + _vrOnly);
        }

        [MenuItem(MenuItem6Dof)]
        private static void Toggle6Dof()
        {
            _sixDof = !_sixDof;

            ManifestTagInfo tagInfo = GetManifestTagInfo<bool>(out YVRSDKSettingAsset asset, "android.hardware.vr.headtracking", CreateTrackingModeInfo, true);
            tagInfo.attrs = CreateTrackingModeAttrs(_sixDof);

            Menu.SetChecked(MenuItem6Dof, _sixDof);

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void OnDelayCall()
        {
            _sixDof = GetManifestTagInfo<bool>(out YVRSDKSettingAsset asset, "android.hardware.vr.headtracking", CreateTrackingModeInfo, true).attrs.Contains("true");
            _vrOnly = GetVrOnlyTagInfo(out asset, true).required;
            ManifestTagInfo immerseHMDTagInfo = GetImmerseHMDTagInfo(out asset);
            immerseHMDTagInfo.required = _vrOnly;
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            Menu.SetChecked(MenuItem6Dof, _sixDof);
            Menu.SetChecked(MenuItemVROnly, _vrOnly);
        }

        private static ManifestTagInfo GetImmerseHMDTagInfo(out YVRSDKSettingAsset asset)
        {
            ManifestTagInfo tagInfo;
            if (File.Exists(AssetFilePath))
            {
                asset = AssetDatabase.LoadAssetAtPath<YVRSDKSettingAsset>(SettingAssetPath);
                asset.manifestTagInfosList ??= new List<ManifestTagInfo>();
                tagInfo =
                    asset.manifestTagInfosList.Find(info => info.attrValue == "org.khronos.openxr.intent.category.IMMERSIVE_HMD");
                if (tagInfo == null)
                {
                    tagInfo = CreateImmerseHMDTagInfo(true);
                    asset.manifestTagInfosList.Add(tagInfo);
                }
            }
            else
            {
                asset = ScriptableObject.CreateInstance<YVRSDKSettingAsset>();
                tagInfo = CreateImmerseHMDTagInfo(true);
                asset.manifestTagInfosList = new List<ManifestTagInfo>() { tagInfo };
                ScriptableObjectUtility.CreateAsset(asset, SaveDataPath);
            }

            return tagInfo;
        }

        private static ManifestTagInfo CreateImmerseHMDTagInfo(bool required)
        {
            return new ManifestTagInfo()
            {
                nodePath = "/manifest/application/activity/intent-filter",
                tag = "category",
                attrName = "name",
                attrValue = "org.khronos.openxr.intent.category.IMMERSIVE_HMD",
                attrs = new string[0],
                required = required,
                modifyIfFound = true
            };
        }

        private static ManifestTagInfo GetVrOnlyTagInfo(out YVRSDKSettingAsset asset, bool required = true)
        {
            ManifestTagInfo tagInfo;
            if (File.Exists(AssetFilePath))
            {
                asset = AssetDatabase.LoadAssetAtPath<YVRSDKSettingAsset>(SettingAssetPath);
                asset.manifestTagInfosList ??= new List<ManifestTagInfo>();
                tagInfo =
                    asset.manifestTagInfosList.Find(info => info.attrValue == "com.yvr.application.mode");
                if (tagInfo == null)
                {
                    tagInfo = CreateVrOnlyInfo(required);
                    asset.manifestTagInfosList.Add(tagInfo);
                }
            }
            else
            {
                asset = ScriptableObject.CreateInstance<YVRSDKSettingAsset>();
                tagInfo = CreateVrOnlyInfo(true);
                asset.manifestTagInfosList = new List<ManifestTagInfo>() { tagInfo };
                ScriptableObjectUtility.CreateAsset(asset, SaveDataPath);
            }

            return tagInfo;
        }

        private static ManifestTagInfo CreateVrOnlyInfo(bool required)
        {
            return new ManifestTagInfo()
            {
                nodePath = "/manifest/application",
                tag = "meta-data",
                attrName = "name",
                attrValue = "com.yvr.application.mode",
                attrs = new[] { "value", "vr_only" },
                required = required,
                modifyIfFound = true
            };
        }

        private static ManifestTagInfo CreateTrackingModeInfo(bool is6Dof)
        {
            return new ManifestTagInfo()
            {
                nodePath = "/manifest",
                tag = "uses-feature",
                attrName = "name",
                attrValue = "android.hardware.vr.headtracking",
                attrs = CreateTrackingModeAttrs(is6Dof),
                required = true,
                modifyIfFound = true
            };
        }

        private static string[] CreateTrackingModeAttrs(bool is6Dof)
        {
            return new[] { "version", "1", "required", is6Dof ? "true" : "false" };
        }

        private void GenerateSplashScreenImageAsset(string path)
        {
            YVRXRSettings settings = null;
            UnityEditor.EditorBuildSettings.TryGetConfigObject<YVRXRSettings>("YVR.Core.XR.YVRXRSettings", out settings);

            YVRSDKSettingAsset asset = GetSettingAsset();
            string splashScreenAssetFolder = Path.Combine(path, "src/main/assets");
            string splashScreenAsssetPath = splashScreenAssetFolder + "/vr_splash.png";
            if (settings != null && settings.OSSplashScreen != null)
            {
                string splashScreenSourcePath = AssetDatabase.GetAssetPath(settings.OSSplashScreen);

                asset.assetInfoList ??= new List<AssetInfo>();
                AssetInfo assetInfo = asset.assetInfoList.Find(info => info.androidProjectAssetPath == splashScreenAsssetPath);
                if (assetInfo == null)
                {
                    assetInfo = new AssetInfo() { unityAssetPath = splashScreenSourcePath, androidProjectAssetPath = splashScreenAsssetPath, shouldDelete = false };
                    asset.assetInfoList.Add(assetInfo);
                }
                else
                {
                    int index = asset.assetInfoList.IndexOf(assetInfo);
                    asset.assetInfoList[index].unityAssetPath = splashScreenSourcePath;
                    asset.assetInfoList[index].androidProjectAssetPath = splashScreenAsssetPath;
                    asset.assetInfoList[index].shouldDelete = false;
                }
            }
            else
            {
                asset.assetInfoList ??= new List<AssetInfo>();
                AssetInfo shouldDeleteInfo = asset.assetInfoList.Find(info => info.androidProjectAssetPath == splashScreenAsssetPath);
                if (shouldDeleteInfo != null)
                {
                    int index = asset.assetInfoList.IndexOf(shouldDeleteInfo);
                    asset.assetInfoList[index].shouldDelete = true;
                }
            }

            asset.manifestTagInfosList ??= new List<ManifestTagInfo>();

            ManifestTagInfo manifestTagInfo = CreateSplashScreenInfo(settings != null && settings.OSSplashScreen != null);
            ManifestTagInfo tagInfo = asset.manifestTagInfosList.Find(info => info.attrValue == "com.yvr.ossplash");
            if (tagInfo == null)
            {
                asset.manifestTagInfosList.Add(manifestTagInfo);
            }
            else
            {
                int index = asset.manifestTagInfosList.IndexOf(tagInfo);
                asset.manifestTagInfosList[index].attrs = manifestTagInfo.attrs;
            }


            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
        }

        private ManifestTagInfo CreateSplashScreenInfo(bool required)
        {
            return new ManifestTagInfo()
            {
                nodePath = "/manifest/application",
                tag = "meta-data",
                attrName = "name",
                attrValue = "com.yvr.ossplash",
                attrs = new[] { "value", required ? "true" : "false" },
                required = true,
                modifyIfFound = true
            };
        }

        private void GeneratePackageListMetaInfo()
        {
            ListRequest listRequest = Client.List(true, true);
            int waitCount = 0;
            while (listRequest.Status != StatusCode.Success && waitCount < 10)
            {
                waitCount++;
                Thread.Sleep(1000);
            }

            if (listRequest.Status != StatusCode.Success) return;

            var yvrPackagesInfo = listRequest.Result.Where(item => item.name.Contains("com.yvr")).Select(item => string.Format("Unity_{0}_{1}", item.name.Substring("com.yvr.".Length), item.version)).ToArray();
            string packageInfoStr = string.Join("|", yvrPackagesInfo);

            ManifestTagInfo manifstTagInfo = GetManifestTagInfo<string>(out YVRSDKSettingAsset asset, "yvr.sdk.version", CreateYVRPackageVersionInfo, packageInfoStr);
            manifstTagInfo.attrs = new string[] { "value", packageInfoStr };

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private ManifestTagInfo CreateYVRPackageVersionInfo(string packageInfoStr)
        {
            return new ManifestTagInfo()
            {
                nodePath = "/manifest/application",
                tag = "meta-data",
                attrName = "name",
                attrValue = "yvr.sdk.version",
                attrs = new[] { "value", packageInfoStr },
                required = true,
                modifyIfFound = true
            };
        }

        private static ManifestTagInfo GetManifestTagInfo<T>(out YVRSDKSettingAsset asset, string condition, Func<T, ManifestTagInfo> CreateFunc, T param)
        {
            asset = GetSettingAsset();
            ManifestTagInfo manifestTagInfo = null;
            asset = AssetDatabase.LoadAssetAtPath<YVRSDKSettingAsset>(SettingAssetPath);
            asset.manifestTagInfosList ??= new List<ManifestTagInfo>();
            manifestTagInfo = asset.manifestTagInfosList.Find(info => info.attrValue == condition);
            if (manifestTagInfo == null)
            {
                manifestTagInfo = CreateFunc.Invoke(param);
                asset.manifestTagInfosList.Add(manifestTagInfo);
            }
            return manifestTagInfo;
        }

        private static YVRSDKSettingAsset GetSettingAsset()
        {
            if (File.Exists(AssetFilePath))
            {
                return AssetDatabase.LoadAssetAtPath<YVRSDKSettingAsset>(SettingAssetPath);
            }
            else
            {
                YVRSDKSettingAsset asset = ScriptableObject.CreateInstance<YVRSDKSettingAsset>();
                ScriptableObjectUtility.CreateAsset(asset, SaveDataPath);
                return asset;
            }
        }

        private void CopyAsset()
        {
            if (File.Exists(AssetFilePath))
            {
                YVRSDKSettingAsset asset = AssetDatabase.LoadAssetAtPath<YVRSDKSettingAsset>(SettingAssetPath);
                if (asset.assetInfoList != null)
                {
                    AssetPreprocessor.CopyAsset(asset.assetInfoList);
                }
            }
        }

        private void PatchAndroidManifest(string path)
        {
            string manifestFolder = Path.Combine(path, "src/main");
            string file = manifestFolder + "/AndroidManifest.xml";

            if (File.Exists(AssetFilePath))
            {
                YVRSDKSettingAsset asset = AssetDatabase.LoadAssetAtPath<YVRSDKSettingAsset>(SettingAssetPath);
                if (asset.manifestTagInfosList != null)
                    ManifestPreprocessor.PatchAndroidManifest(asset.manifestTagInfosList, file);
            }
            else
            {
                Debug.LogError($"{AssetFilePath} is not exit");
            }
        }

        #region build callback

        int IOrderedCallback.callbackOrder => 9999;

        void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path)
        {
            GeneratePackageListMetaInfo();
            GenerateSplashScreenImageAsset(path);
            CopyAsset();
            PatchAndroidManifest(path);
        }

        #endregion build callback
    }
}