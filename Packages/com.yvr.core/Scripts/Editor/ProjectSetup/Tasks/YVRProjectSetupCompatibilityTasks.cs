using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Management;

namespace YVR.Core.Editor
{
    [InitializeOnLoad]
    public static class YVRProjectSetupCompatibilityTasks
    {
        static YVRProjectSetupCompatibilityTasks()
        {
            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Compatibility,
                level: YVRProjectSetup.TaskLevel.Required,
                category: YVRProjectSetup.TaskCategory.ProjectSetup,
                isDone: () => { return BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) == BuildTargetGroup.Android; },
                issueMessage: $"Build Target {BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget)} is not supported",
                fixedMessage: $"Build Target {BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget)} is supported",
                fixAction: null,
                documentationUrl: "https://developer.yvrdream.com/yvrdoc/unity_CN/UserManual_CN/GetStarted/BuildYourFirstApp.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Compatibility,
                level: YVRProjectSetup.TaskLevel.Required,
                category: YVRProjectSetup.TaskCategory.ProjectSetup,
                isDone: () => PlayerSettings.Android.minSdkVersion >= AndroidSdkVersions.AndroidApiLevel25,
                issueMessage: "Minimum Android API Level must be at least 25",
                fixedMessage: "PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel25",
                fixAction: () => { PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel25; },
                documentationUrl: "https://developer.yvrdream.com/yvrdoc/unity_CN/UserManual_CN/GetStarted/ConfigureDevelopmentEnvironment.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Compatibility,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.ProjectSetup,
                isDone: () => PlayerSettings.Android.targetSdkVersion == AndroidSdkVersions.AndroidApiLevelAuto || PlayerSettings.Android.targetSdkVersion >= AndroidSdkVersions.AndroidApiLevel25,
                issueMessage: "Target API should be set to \"Automatic\" as to ensure latest version",
                fixedMessage: "PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto",
                fixAction: () => { PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto; },
                documentationUrl: "https://developer.yvrdream.com/yvrdoc/unity_CN/UserManual_CN/GetStarted/ConfigureDevelopmentEnvironment.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Compatibility,
                level: YVRProjectSetup.TaskLevel.Required,
                category: YVRProjectSetup.TaskCategory.ProjectSetup,
                isDone: () => PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android) == ScriptingImplementation.IL2CPP,
                issueMessage: "Building the ARM64 architecture requires using IL2CPP as the scripting backend",
                fixedMessage: "PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.IL2CPP)",
                fixAction: () => { PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP); },
                documentationUrl: "https://developer.yvrdream.com/yvrdoc/unity_CN/UserManual_CN/GetStarted/ConfigureUnitySettings.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Compatibility,
                level: YVRProjectSetup.TaskLevel.Required,
                category: YVRProjectSetup.TaskCategory.ProjectSetup,
                isDone: () => (PlayerSettings.Android.targetArchitectures & AndroidArchitecture.ARM64) != 0,
                issueMessage: "Use ARM64 as target architecture",
                fixedMessage: "PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64",
                fixAction: () => { PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64; },
                documentationUrl: "https://developer.yvrdream.com/yvrdoc/unity_CN/UserManual_CN/GetStarted/ConfigureUnitySettings.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Compatibility,
                level: YVRProjectSetup.TaskLevel.Required,
                category: YVRProjectSetup.TaskCategory.ProjectSetup,
                isDone: () =>
                {
                    try
                    {
                        Assembly assembly = Assembly.Load("Unity.XR.Management.Editor");
                        System.Type entitledManagerType = assembly.GetType("UnityEditor.XR.Management.Metadata.XRPackageMetadataStore");
                        MethodInfo method = entitledManagerType.GetMethod("IsLoaderAssigned", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string), typeof(BuildTargetGroup)}, null);
                        bool result = (bool)method.Invoke(entitledManagerType, new object[] { "YVR.Core.XR.YVRXRLoader", BuildTargetGroup.Android });
                        return result;
                    }
                    catch (System.Exception e)
                    {
                        return false;
                    }
                },
                issueMessage: "YVRLoader not assign in XR Plug-in Management, Please toggle on Edit->ProjectSettings->XR Plug-in Management->Android Tab->YVR.",
                fixedMessage: "YVRLoader has been assigend.",
                fixAction: () =>
                {
                    var settings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Android);

                    if (settings == null)
                        return;

                    if (settings.AssignedSettings == null)
                    {
                        var assignedSettings = ScriptableObject.CreateInstance<XRManagerSettings>() as XRManagerSettings;
                        settings.AssignedSettings = assignedSettings;
                        EditorUtility.SetDirty(settings);
                    }

                    XRPackageMetadataStore.AssignLoader(settings.AssignedSettings, "YVR.Core.XR.YVRXRLoader", BuildTargetGroup.Android);

                },
                documentationUrl: "https://developer.yvrdream.com/yvrdoc/unity_CN/UserManual_CN/GetStarted/ConfigureUnitySettings.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Compatibility,
                level: YVRProjectSetup.TaskLevel.Required,
                category: YVRProjectSetup.TaskCategory.ProjectSetup,
                isDone: () =>
                {
                    return !PlayerSettings.Android.androidTVCompatibility;
                },
                issueMessage: "androidTVCompatibility is not support",
                fixedMessage: "PlayerSettings.Android.androidTVCompatibility = false",
                fixAction: () =>
                {
                    PlayerSettings.Android.androidTVCompatibility = false;

                },
                documentationUrl: null
            );
        }


        private static GraphicsDeviceType[] GetGraphicsAPIs()
        {
            if (PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.Android))
            {
                return Array.Empty<GraphicsDeviceType>(); ;
            }

            return PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
        }
    }
}