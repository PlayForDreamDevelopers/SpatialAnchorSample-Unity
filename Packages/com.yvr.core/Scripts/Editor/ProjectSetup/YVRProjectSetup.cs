using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace YVR.Core.Editor
{
    public static class YVRProjectSetup
    {
        public enum TaskLevel
        {
            Optional = 0,
            Recommended = 1,
            Required = 2
        }

        public enum TaskGroup
        {
            All = 0,
            Compatibility = 1,
            Rendering = 2,
            Quality = 3,
            Physics = 4,
            Packages = 5,
            Features = 6,
            Miscellaneous = 7
        }

        public enum TaskCategory
        {
            ProjectSetup,
            Performance,
            Porting
        }

        public static YVRConfigurationSummary s_Summary;
        static YVRConfigurationTaskRegistry s_Registry;
        static Queue<YVRConfigurationTaskProcessor> m_ProcessorQueue = new Queue<YVRConfigurationTaskProcessor>();
        static bool s_Busy => m_ProcessorQueue.Count > 0;

        private static readonly double s_UpdateTimeSpan = 5f;
        private static double updateTimer = 0f;

        static YVRProjectSetup()
        {
            s_Registry = new YVRConfigurationTaskRegistry();
            s_Summary = new YVRConfigurationSummary(s_Registry);
            EditorSceneManager.sceneOpened += OnSceneOpend;
            EditorApplication.update += OnEditorApplicationUpdate;
        }

        private static void OnSceneOpend(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            s_Summary.Update();
        }

        private static void OnEditorApplicationUpdate()
        {
            var currentTime = EditorApplication.timeSinceStartup;
            if (currentTime - updateTimer > s_UpdateTimeSpan)
            {
                updateTimer = currentTime;
                s_Summary.Update();
            }
        }

        public static void AddTask(TaskGroup group, TaskLevel level, TaskCategory category, Func<bool> isDone, Action fixAction, string issueMessage, string fixedMessage, string documentationUrl)
        {
            YVRConfigurationTask task = new YVRConfigurationTask(group, level, category, isDone, fixAction, issueMessage, fixedMessage, documentationUrl);
            s_Registry.AddTask(task);
        }

        public static void FixTasks(Func<IEnumerable<YVRConfigurationTask>, List<YVRConfigurationTask>> filter, Action<YVRConfigurationTaskProcessor> onComplete)
        {
            var processor = new YVRConfigurationTaskProcessor(filter, s_Registry, onComplete);
            Enqueue(processor);
        }

        public static void FixTask(YVRConfigurationTask task, Action<YVRConfigurationTaskProcessor> onComplete)
        {
            var filter = (Func<IEnumerable<YVRConfigurationTask>, List<YVRConfigurationTask>>)(tasks => tasks.Where(otherTask => otherTask == task).ToList());
            var processor = new YVRConfigurationTaskProcessor(filter, s_Registry, onComplete);
            Enqueue(processor);
        }

        public static IEnumerable<YVRConfigurationTask> GetTasks()
        {
            return s_Registry.GetTasks();
        }

        private static void Enqueue(YVRConfigurationTaskProcessor processor)
        {
            if (!s_Busy)
            {
                EditorApplication.update += Update;
            }
            m_ProcessorQueue.Enqueue(processor);
        }

        private static void Dequeue(YVRConfigurationTaskProcessor processor)
        {
            if (processor != m_ProcessorQueue.Peek())
            {
                return;
            }

            s_Summary.Update();
            YVRConfigurationTaskProcessor dequeuedProcessor = m_ProcessorQueue.Dequeue();

            if (!s_Busy)
            {
                EditorApplication.update -= Update;
            }
        }

        private static void Update()
        {
            while (m_ProcessorQueue.Count > 0)
            {
                var current = m_ProcessorQueue.Peek();
                if (!current.started)
                {
                    current.Start();
                }

                current.Update();
                if (current.completed)
                {
                    Dequeue(current);
                }
                else
                {
                    return;
                }
            }
        }
    }
}