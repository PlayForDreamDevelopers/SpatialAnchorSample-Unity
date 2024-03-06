using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using YVR.Core.XR;

namespace YVR.Core.Editor
{
    [InitializeOnLoad]
    public static class YVRProjectSetupRenderingTasks
    {
        static YVRProjectSetupRenderingTasks()
        {
            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return PlayerSettings.colorSpace == ColorSpace.Linear; },
                issueMessage: $"Color Space is recommand to be Linear",
                fixedMessage: $"PlayerSettings.colorSpace = ColorSpace.Linear",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/cn/current/Manual/LinearLighting.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return !PlayerSettings.graphicsJobs; },
                issueMessage: $"Disable Graphics Jobs",
                fixedMessage: $"PlayerSettings.graphicsJobs = false",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2021.3/Documentation/ScriptReference/PlayerSettings-graphicsJobs.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return PlayerSettings.MTRendering && PlayerSettings.GetMobileMTRendering(BuildTargetGroup.Android); },
                issueMessage: $"Enable Multithreaded Rendering",
                fixedMessage: $"PlayerSettings.MTRendering = true and PlayerSettings.SetMobileMTRendering(buildTargetGroup, true)",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return PlayerSettings.use32BitDisplayBuffer; },
                issueMessage: $"Use 32Bit Display Buffer",
                fixedMessage: $"PlayerSettings.use32BitDisplayBuffer = true",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.Android, Graphics.activeTier).renderingPath == RenderingPath.Forward; },
                issueMessage: $"Use Forward Rendering Path",
                fixedMessage: $"renderingTier.renderingPath = RenderingPath.Forward",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2021.3/Documentation/Manual/RenderingPaths.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Optional,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    return LightmapSettings.lightmaps.Length == 0 ||
                           LightmapSettings.lightmapsMode == LightmapsMode.NonDirectional;
                },
                issueMessage: $"Use Non-Directional lightmaps",
                fixedMessage: $"LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2021.3/Documentation/ScriptReference/LightmapsMode.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Optional,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return !Lightmapping.realtimeGI; },
                issueMessage: $"Disable Realtime Global Illumination",
                fixedMessage: $"Lightmapping.realtimeGI = false",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2021.3/Documentation/ScriptReference/LightingSettings-realtimeGI.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Optional,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return PlayerSettings.gpuSkinning; },
                issueMessage: $"Consider using GPU Skinning if your application is CPU bound",
                fixedMessage: $"PlayerSettings.gpuSkinning = true",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2021.3/Documentation/ScriptReference/PlayerSettings-gpuSkinning.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var materials = Resources.FindObjectsOfTypeAll<Material>();
                    bool containParallaxMappedMaterials = false;
                    for (int i = 0; i < materials.Length; ++i)
                    {
                        if (materials[i].shader.name.Contains("Parallax") || materials[i].IsKeywordEnabled("_PARALLAXMAP"))
                        {
                            containParallaxMappedMaterials = true;
                            break;
                        }
                    }
                    return !containParallaxMappedMaterials;
                },
                issueMessage: $"For GPU performance, please don't use parallax-mapped materials.",
                fixedMessage: $"For GPU performance, please don't use parallax-mapped materials.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var materials = Resources.FindObjectsOfTypeAll<Material>();
                    bool containSpecularMaterials = false;
                    for (int i = 0; i < materials.Length; ++i)
                    {
                        if (materials[i].IsKeywordEnabled("_SPECGLOSSMAP") || materials[i].IsKeywordEnabled("_METALLICGLOSSMAP"))
                        {
                            containSpecularMaterials = true;
                            break;
                        }
                    }
                    return !containSpecularMaterials;
                },
                issueMessage: $"For GPU performance, please don't use specular shader on materials.",
                fixedMessage: $"For GPU performance, please don't use specular shader on materials.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var materials = Resources.FindObjectsOfTypeAll<Material>();
                    bool haveTooManyPasses = false;
                    for (int i = 0; i < materials.Length; ++i)
                    {
                        if (materials[i].passCount > 2)
                        {
                            haveTooManyPasses = true;
                            break;
                        }
                    }
                    return !haveTooManyPasses;
                },
                issueMessage: $"Please use 2 or fewer passes in materials.",
                fixedMessage: $"Please use 2 or fewer passes in materials.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var compositeLayers = GameObject.FindObjectsOfType<YVRCompositeLayer>();
                    return compositeLayers.Length < 4;
                },
                issueMessage: $"For GPU performance, please use 4 or fewer CompositeLayers.",
                fixedMessage: $"For GPU performance, please use 4 or fewer CompositeLayers.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var splashScreen = PlayerSettings.virtualRealitySplashScreen;
                    return splashScreen == null || (splashScreen.filterMode == FilterMode.Trilinear && splashScreen.mipmapCount > 1);
                },
                issueMessage: $"For visual quality, please use trilinear filtering and mipmap on your VR splash screen.",
                fixedMessage: $"For visual quality, please use trilinear filtering and mipmap on your VR splash screen.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    return YVRManager.createdInstance != null && YVRManager.createdInstance.useRecommendedMSAALevel;
                },
                issueMessage: $"Recommend enabling useRecommendedMSAALevel in YVRManager",
                fixedMessage: $"Recommend enabling useRecommendedMSAALevel in YVRManager",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    YVRXRSettings settings = null;
                    UnityEditor.EditorBuildSettings.TryGetConfigObject<YVRXRSettings>("YVR.Core.XR.YVRXRSettings", out settings);
                    return settings != null && settings.eyeResolutionScale < 1.5f;
                },
                issueMessage: $"Render scale above 1.5 is extremely expensive on the GPU, with little if any positive visual benefit.",
                fixedMessage: $"Render scale above 1.5 is extremely expensive on the GPU, with little if any positive visual benefit.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    return RenderSettings.skybox == null;
                },
                issueMessage: $"For GPU performance, please don't use Unity's built-in Skybox.",
                fixedMessage: $"For GPU performance, please don't use Unity's built-in Skybox.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var textures = Resources.FindObjectsOfTypeAll<Texture2D>();
                    bool trillinearWithoutMipmap = false;
                    for (int i = 0; i < textures.Length; ++i)
                    {
                        if (textures[i].filterMode == FilterMode.Trilinear && textures[i].mipmapCount == 1)
                        {
                            trillinearWithoutMipmap = true;
                        }
                    }
                    return !trillinearWithoutMipmap;

                },
                issueMessage: $"For GPU performance, please generate mipmaps or disable trilinear filtering for textures.",
                fixedMessage: $"For GPU performance, please generate mipmaps or disable trilinear filtering for textures.",
                fixAction: null,
                documentationUrl: null
            );
        }
    }
}