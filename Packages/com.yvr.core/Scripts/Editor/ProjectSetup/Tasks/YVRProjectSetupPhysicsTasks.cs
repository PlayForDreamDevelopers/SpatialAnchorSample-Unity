using UnityEditor;
using UnityEngine;

namespace YVR.Core.Editor
{
    [InitializeOnLoad]
    public static class YVRProjectSetupPhysicsTasks
    {
        static YVRProjectSetupPhysicsTasks()
        {
            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Physics,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return Physics.defaultContactOffset >= 0.01f; },
                issueMessage: $"Use Default Context Offset above or equal to 0.01",
                fixedMessage: $"Physics.defaultContactOffset = 0.01f",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2022.1/Documentation/ScriptReference/Physics-defaultContactOffset.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Physics,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return Physics.sleepThreshold >= 0.005f; },
                issueMessage: $"Use Sleep Threshold above or equal to 0.005",
                fixedMessage: $"Physics.sleepThreshold = 0.005f",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2022.1/Documentation/ScriptReference/Physics-sleepThreshold.html"
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Physics,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Performance,
                isDone: () => { return Physics.defaultSolverIterations <= 8; },
                issueMessage: $"Use Default Solver Iteration below or equal to 8",
                fixedMessage: $"Physics.defaultSolverIterations = 8",
                fixAction: null,
                documentationUrl: "https://docs.unity3d.com/2022.1/Documentation/ScriptReference/Physics-defaultSolverIterations.html"
            );
        }
    }
}