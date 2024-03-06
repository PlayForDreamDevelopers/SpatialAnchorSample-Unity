using UnityEditor;

namespace YVR.Core.Editor
{
    [InitializeOnLoad]
    public static class YVRProjectSetupRuntimeTasks
    {
        static YVRProjectSetupRuntimeTasks()
        {
            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    return UnityStats.usedTextureMemorySize + UnityStats.vboTotalBytes <= 1000000;
                },
                issueMessage: $"Please use less than 1GB of vertex and texture memory.",
                fixedMessage: $"Please use less than 1GB of vertex and texture memory..",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    return UnityStats.triangles <= 100000 && UnityStats.vertices <= 100000;
                },
                issueMessage: $"Please use less than 100000 triangles or vertices.",
                fixedMessage: $"Please use less than 100000 triangles or vertices.",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () =>
                {
                    return UnityStats.drawCalls <= 100;
                },
                issueMessage: $"Please use less than 100 draw calls.",
                fixedMessage: $"Please use less than 100 draw calls.",
                fixAction: null,
                documentationUrl: null
            );
        }
    }
}