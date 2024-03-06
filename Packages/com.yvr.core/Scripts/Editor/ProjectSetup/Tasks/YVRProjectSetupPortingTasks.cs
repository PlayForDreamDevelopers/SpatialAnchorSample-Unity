using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace YVR.Core.Editor
{
    [InitializeOnLoad]
    public static class YVRProjectSetupPortingTasks
    {
        static YVRProjectSetupPortingTasks()
        {
            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Porting,
                isDone: () =>
                {
                    return !CheckScriptExistInScene("Unity.XR.PICO", "Unity.XR.PXR.PXR_Manager");

                },
                issueMessage: $"PXR_Manager exist in scene",
                fixedMessage: $"PXR_Manager not exist in scene",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Porting,
                isDone: () =>
                {
                    return !CheckScriptExistInScene("Oculus.VR", "OVRManager");

                },
                issueMessage: $"OVRManager exist in scene",
                fixedMessage: $"OVRManager not exist in scene",
                fixAction: null,
                documentationUrl: null
            );

            YVRProjectSetup.AddTask(
                group: YVRProjectSetup.TaskGroup.Rendering,
                level: YVRProjectSetup.TaskLevel.Recommended,
                category: YVRProjectSetup.TaskCategory.Porting,
                isDone: () =>
                {
                    return CheckScriptExistInScene("YVR.Platform.Runtime", "YVR.Platform.YVREntitledManager");

                },
                issueMessage: $"YVREntitledManager script not exist in scene, make sure you have written entitled related code",
                fixedMessage: $"YVREntitledManager exist in scene",
                fixAction: null,
                documentationUrl: "https://developer.yvrdream.com/yvrdoc/unity_CN/Documentation_CN~/EntitlementCheck.html"
            );
        }

        private static bool CheckScriptExistInScene(string assemblyName, string className)
        {
            var monoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();
            bool classExist = false;
            try
            {
                Assembly assembly = Assembly.Load(assemblyName);
                System.Type entitledManagerType = assembly.GetType(className);
                if (entitledManagerType != null)
                {
                    classExist = monoBehaviours.Any(item => item.GetType() == entitledManagerType);
                }
                return classExist;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
    }
}