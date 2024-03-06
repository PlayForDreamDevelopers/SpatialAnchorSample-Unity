using UnityEditor;
using UnityEngine;

namespace YVR.Core.Editor
{
    [InitializeOnLoad]
    public static class YVRProjectSetupQualityTasks
    {
        static YVRProjectSetupQualityTasks()
        {
            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Quality,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return QualitySettings.pixelLightCount <= 1; },
                issueMessage: $"Set maximum pixel lights count to 1",
                fixedMessage: $"QualitySettings.pixelLightCount = 1",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2022.1/Documentation/ScriptReference/QualitySettings-pixelLightCount.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Quality,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                issueMessage: $"Set Texture Quality to Full Res",
#if UNITY_2022_2_OR_NEWER
            isDone: () => { return QualitySettings.globalTextureMipmapLimit == 0; },
            fixedMessage: $"QualitySettings.globalTextureMipmapLimit = 0",
            fixAction: null,
#else
                isDone: () => { return QualitySettings.masterTextureLimit == 0; },
                fixedMessage: $"QualitySettings.masterTextureLimit = 0",
                fixAction: null,
#endif
                documentationUrl: "https://docs.unity3d.com/2022.1/Documentation/ScriptReference/QualitySettings-masterTextureLimit.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Quality,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable; },
                issueMessage: $"Enable Anisotropic Filtering on a per-texture basis",
                fixedMessage: $"QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2022.1/Documentation/ScriptReference/AnisotropicFiltering.Enable.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Quality,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return EditorUserBuildSettings.androidBuildSubtarget == MobileTextureSubtarget.ASTC || EditorUserBuildSettings.androidBuildSubtarget == MobileTextureSubtarget.ETC2; },
                issueMessage: $"Optimize Texture Compression : For GPU performance, please use ETC2. In some cases, ASTC may produce better visuals and is also a viable solution.",
                fixedMessage: $"EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ETC2",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2022.1/Documentation/ScriptReference/MobileTextureSubtarget.html"
            );
        }
    }
}