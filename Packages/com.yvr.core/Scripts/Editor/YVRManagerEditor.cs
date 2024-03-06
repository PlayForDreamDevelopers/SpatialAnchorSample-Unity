using System;
using System.Collections.Generic;
using System.IO;
using AndroidManifest;
using UnityEditor;
using UnityEngine;

namespace YVR.Core.XR
{
    [CustomEditor(typeof(YVRManager))]
    public class YVRManagerEditor : UnityEditor.Editor
    {
        private const string k_SaveDataPath = "Assets/XR/Resources/";
        private static readonly string s_SettingAssetPath = $"{k_SaveDataPath}{typeof(YVRSDKSettingAsset)}.asset";
        private SerializedProperty m_HandTrackingSupport;
        private SerializedProperty m_SpatialAnchorSupport;
        private SerializedProperty m_SceneSupport;

        private void OnEnable()
        {
            m_HandTrackingSupport = serializedObject.FindProperty("handTrackingSupport");
            m_SpatialAnchorSupport = serializedObject.FindProperty("spatialAnchorSupport");
            m_SceneSupport = serializedObject.FindProperty("sceneSupport");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject,m_SpatialAnchorSupport.propertyPath,m_HandTrackingSupport.propertyPath,m_SceneSupport.propertyPath);
            EditorGUI.BeginChangeCheck();
            var curHandtrackingValue = m_HandTrackingSupport.intValue;
            var curSpatialAnchorValue = m_SpatialAnchorSupport.boolValue;
            var curSceneSupportValue = m_SceneSupport.boolValue;
            EditorGUILayout.PropertyField(m_HandTrackingSupport);
            if (curSceneSupportValue)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(m_SpatialAnchorSupport);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.PropertyField(m_SpatialAnchorSupport);
            }
            EditorGUILayout.PropertyField(m_SceneSupport);
            if (m_HandTrackingSupport.intValue != curHandtrackingValue)
            {
                SetHandTrackingInfo(out YVRSDKSettingAsset asset);
                PatchAndroidManifest(asset);
            }

            if (m_SceneSupport.boolValue != curSceneSupportValue)
            {
                if (m_SceneSupport.boolValue)
                {
                    m_SpatialAnchorSupport.boolValue = true;
                }
                SetSceneInfo(out YVRSDKSettingAsset asset);
                PatchAndroidManifest(asset);
            }


            if (m_SpatialAnchorSupport.boolValue != curSpatialAnchorValue)
            {
                SetSpatialAnchorInfo(out YVRSDKSettingAsset asset);
                PatchAndroidManifest(asset);
            }


            serializedObject.ApplyModifiedProperties();
        }

        private void PatchAndroidManifest(YVRSDKSettingAsset asset)
        {
            if (asset == null) return;

            var sourceFile = $"Assets/Plugins/Android/AndroidManifest.xml";
            if (File.Exists(sourceFile))
                ManifestPreprocessor.PatchAndroidManifest(asset.manifestTagInfosList, sourceFile);

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private YVRSDKSettingAsset LoadSettingAsset()
        {
            if (File.Exists(s_SettingAssetPath))
            {
                var asset = AssetDatabase.LoadAssetAtPath<YVRSDKSettingAsset>(s_SettingAssetPath);
                if (asset == null)
                {
                    Debug.LogError("Load YVRSDKSettingAsset failed! Please check the file: " + s_SettingAssetPath);
                }
                return asset;
            }

            return CreateSettingAsset();
        }

        private YVRSDKSettingAsset CreateSettingAsset()
        {
            var asset = CreateInstance<YVRSDKSettingAsset>();
            ScriptableObjectUtility.CreateAsset(asset, k_SaveDataPath);
            return asset;
        }

        private void SetHandTrackingInfo(out YVRSDKSettingAsset asset)
        {
            asset = LoadSettingAsset();
            if (asset == null) return;

            ManifestTagInfo permissionTagInfo =
                asset.manifestTagInfosList.Find(info => info.attrValue == "com.yvr.permission.HAND_TRACKING");
            ManifestTagInfo handTrackingTagInfo =
                asset.manifestTagInfosList.Find(info => info.attrValue == "yvr.software.handtracking");

            asset.manifestTagInfosList ??= new List<ManifestTagInfo>();
            if (permissionTagInfo == null)
            {
                permissionTagInfo = CreateHandTrackingPermission();
                asset.manifestTagInfosList.Add(permissionTagInfo);
            }

            permissionTagInfo.required = m_HandTrackingSupport.intValue !=
                                         (int)YVRManager.HandTrackingSupport.ControllersOnly;
            bool isHandsOnly =  m_HandTrackingSupport.intValue == (int)YVRManager.HandTrackingSupport.HandsOnly;
            if (handTrackingTagInfo == null)
            {
                handTrackingTagInfo = CreateHandTrackingInfo(isHandsOnly);
                asset.manifestTagInfosList.Add(handTrackingTagInfo);
            }

            handTrackingTagInfo.attrs = new[] { "required", isHandsOnly.ToString().ToLower() };
            handTrackingTagInfo.required =  m_HandTrackingSupport.intValue !=
                                            (int)YVRManager.HandTrackingSupport.ControllersOnly;

        }

        private void SetSpatialAnchorInfo(out YVRSDKSettingAsset asset)
        {
            asset = LoadSettingAsset();
            if (asset == null) return;

            ManifestTagInfo permissionTagInfo =
                asset.manifestTagInfosList.Find(info => info.attrValue == "com.yvr.permission.USE_ANCHOR_API");

            asset.manifestTagInfosList ??= new List<ManifestTagInfo>();
            if (permissionTagInfo == null)
            {
                permissionTagInfo = CreateSpatialAnchorPermission();
                asset.manifestTagInfosList.Add(permissionTagInfo);
            }

            permissionTagInfo.required = m_SpatialAnchorSupport.boolValue;
        }

        private void SetSceneInfo(out YVRSDKSettingAsset asset)
        {
            asset = LoadSettingAsset();
            if (asset == null) return;

            ManifestTagInfo permissionTagInfo =
                asset.manifestTagInfosList.Find(info => info.attrValue == "com.yvr.permission.USE_SCENE");

            asset.manifestTagInfosList ??= new List<ManifestTagInfo>();
            if (permissionTagInfo == null)
            {
                permissionTagInfo = CreateScenePermission();
                asset.manifestTagInfosList.Add(permissionTagInfo);
            }

            permissionTagInfo.required = m_SceneSupport.boolValue;
        }

        private ManifestTagInfo CreateScenePermission()
        {
            return new ManifestTagInfo()
            {
                nodePath = "/manifest",
                tag = "uses-permission",
                attrName = "name",
                attrValue = "com.yvr.permission.USE_SCENE",
                modifyIfFound = true
            };
        }

        private ManifestTagInfo CreateSpatialAnchorPermission()
        {
            return new ManifestTagInfo()
            {
                nodePath = "/manifest",
                tag = "uses-permission",
                attrName = "name",
                attrValue = "com.yvr.permission.USE_ANCHOR_API",
                modifyIfFound = true
            };
        }

        private ManifestTagInfo CreateHandTrackingPermission()
        {
            return new ManifestTagInfo()
            {
                nodePath = "/manifest",
                tag = "uses-permission",
                attrName = "name",
                attrValue = "com.yvr.permission.HAND_TRACKING",
                modifyIfFound = true
            };
        }

        private ManifestTagInfo CreateHandTrackingInfo(bool required)
        {
            Debug.Log(required.ToString().ToLower());
            return new ManifestTagInfo()
            {
                nodePath = "/manifest",
                tag = "uses-feature",
                attrName = "name",
                attrValue = "yvr.software.handtracking",
                attrs = new[] { "required", required.ToString().ToLower() },
                modifyIfFound = true
            };
        }
    }
}