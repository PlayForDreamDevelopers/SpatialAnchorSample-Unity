using UnityEditor;
using UnityEngine;
using System.Linq;

namespace YVR.Core.Editor
{
    [InitializeOnLoad]
    public static class YVRProjectSetupSceneTasks
    {
        static YVRProjectSetupSceneTasks()
        {
            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var lights = GameObject.FindObjectsOfType<Light>();
                    bool unbakedLightsExist = lights.Any(light => light.type != LightType.Directional && !light.bakingOutput.isBaked && IsLightBaked(light));
                    return !unbakedLightsExist;
                },
                issueMessage: $"Some lights in scene don't have up to date lightmap data",
                fixedMessage: $"all lights in scene have up to date lightmap data",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var lights = GameObject.FindObjectsOfType<Light>();
                    bool shadowLightsExist = lights.Any(light => light.shadows != LightShadows.None && !IsLightBaked(light));
                    return !shadowLightsExist;
                },
                issueMessage: $"For CPU performance, consider disabling shadows on realtime lights.",
                fixedMessage: $"For CPU performance, consider disabling shadows on realtime lights.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var sources = GameObject.FindObjectsOfType<AudioSource>();
                    int playingAudioSourceCount = 0;

                    if (sources.Length > 16)
                    {
                        playingAudioSourceCount = sources.Where(source => source.isPlaying).Count();
                    }
                    return playingAudioSourceCount <= 16;
                },
                issueMessage: $"For CPU performance, please disable all but the top 16 AudioSources.",
                fixedMessage: $"For CPU performance, please disable all but the top 16 AudioSources.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var sources = GameObject.FindObjectsOfType<AudioSource>();
                    bool isDecompressOnLoad = sources.Any(audioSource => audioSource.clip?.loadType == AudioClipLoadType.DecompressOnLoad);
                    return !isDecompressOnLoad;
                },
                issueMessage: $"For fast loading, please don't use decompress on load for audio clips",
                fixedMessage: $"For fast loading, please don't use decompress on load for audio clips",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var sources = GameObject.FindObjectsOfType<AudioSource>();
                    bool preloadAudioData = sources.Any(audioSource => audioSource.clip?.preloadAudioData == true);
                    return !preloadAudioData;
                },
                issueMessage: $"For fast loading, please don't preload data for audio clips.",
                fixedMessage: $"For fast loading, please don't preload data for audio clips.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var renderers = GameObject.FindObjectsOfType<Renderer>();
                    bool haveInstancedMaterial = renderers.Any(renderer => renderer.sharedMaterial == null);
                    return !haveInstancedMaterial;
                },
                issueMessage: $"Please avoid instanced materials on renderers.",
                fixedMessage: $"Please avoid instanced materials on renderers.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var monoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();
                    bool haveImageEffect = false;
                    System.Type effectBaseType = System.Type.GetType("UnityStandardAssets.ImageEffects.PostEffectsBase");
                    if (effectBaseType != null)
                    {
                        haveImageEffect = monoBehaviours.Any(item => item.GetType().IsSubclassOf(effectBaseType));
                    }
                    return !haveImageEffect;
                },
                issueMessage: $"Please don't use image effects.",
                fixedMessage: $"Please don't use image effects.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var projectors = GameObject.FindObjectsOfType<Projector>();
                    return projectors.Length == 0;
                },
                issueMessage: $"For GPU performance, please don't use projectors.",
                fixedMessage: $"For GPU performance, please don't use projectors.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var cameras = GameObject.FindObjectsOfType<Camera>();
                    int clearCount = cameras.Where(camera => camera.clearFlags != CameraClearFlags.Nothing && camera.clearFlags != CameraClearFlags.Depth).Count();
                    return clearCount <= 2;
                },
                issueMessage: $"Please use 2 or fewer camera clears.",
                fixedMessage: $"Please use 2 or fewer camera clears.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    var cameras = GameObject.FindObjectsOfType<Camera>();
                    bool forceIntoRenderTexture = cameras.Any(camera => camera.forceIntoRenderTexture);
                    return !forceIntoRenderTexture;
                },
                issueMessage: $"For GPU performance, please don't enable forceIntoRenderTexture on your camera",
                fixedMessage: $"For GPU performance, please don't enable forceIntoRenderTexture on your camera",
                fixAction: null,
                documentationUrl: null
            );
        }

        private static bool IsLightBaked(Light light)
        {
            return light.lightmapBakeType == LightmapBakeType.Baked;
        }
    }
}