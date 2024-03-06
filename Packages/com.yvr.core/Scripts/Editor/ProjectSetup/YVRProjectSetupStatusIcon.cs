using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace YVR.Core.Editor
{
    [InitializeOnLoad]
    public static class YVRProjectSetupStatusIcon
    {
        private static readonly Type s_ToolbarType;
        private static readonly PropertyInfo s_GuiBackend;
        private static readonly PropertyInfo s_VisualTree;
        private static readonly FieldInfo s_OnGuiHandler;
        private static readonly GUIContent s_NoIssueIcon;
        private static readonly GUIContent s_OptionalIcon;
        private static readonly GUIContent s_RecommendIcon;
        private static readonly GUIContent s_CriticalIcon;

        private static GUIStyle s_IconStyle;
        private static VisualElement s_Container;
        private static GUIContent s_CurrentIcon;
        private static YVRProjectSetup.TaskLevel s_TaskLevel = default;

        static YVRProjectSetupStatusIcon()
        {
            var editorAssembly = typeof(UnityEditor.Editor).Assembly;
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            s_ToolbarType = editorAssembly.GetType("UnityEditor.AppStatusBar");
            var guiViewType = editorAssembly.GetType("UnityEditor.GUIView");
            var backendType = editorAssembly.GetType("UnityEditor.IWindowBackend");
            var containerType = typeof(IMGUIContainer);

            s_GuiBackend = guiViewType?.GetProperty("windowBackend", bindingFlags);
            s_VisualTree = backendType?.GetProperty("visualTree", bindingFlags);
            s_OnGuiHandler = containerType?.GetField("m_OnGUIHandler", bindingFlags);

            s_NoIssueIcon = YVRProjectSetupResourcesLoader.instance.LoadIcon("ProjectSetup_Fixed.png");
            s_OptionalIcon = YVRProjectSetupResourcesLoader.instance.LoadIcon("ProjectSetup_Optional.png");
            s_RecommendIcon = YVRProjectSetupResourcesLoader.instance.LoadIcon("ProjectSetup_Recommended.png");
            s_CriticalIcon = YVRProjectSetupResourcesLoader.instance.LoadIcon("ProjectSetup_Required.png");
            s_CurrentIcon = s_NoIssueIcon;

            EditorApplication.update += RefreshContainer;
            YVRProjectSetup.s_Summary.onSummaryUpdated += OnProcessorComplete;
        }

        private static void RefreshContainer()
        {
            if (s_Container != null)
            {
                return;
            }
            var toolbars = Resources.FindObjectsOfTypeAll(s_ToolbarType);
            if (toolbars == null || toolbars.Length == 0)
            {
                return;
            }

            var toolbar = toolbars[0];
            if (toolbar == null)
            {
                return;
            }

            var backend = s_GuiBackend?.GetValue(toolbar);
            if (backend == null)
            {
                return;
            }

            var elements = s_VisualTree?.GetValue(backend, null) as VisualElement;
            s_Container = elements?[0];
            if (s_Container == null)
            {
                return;
            }

            var handler = s_OnGuiHandler?.GetValue(s_Container) as Action;
            if (handler == null)
            {
                return;
            }

            handler -= RefreshGUI;
            handler += RefreshGUI;
            s_OnGuiHandler.SetValue(s_Container, handler);

            EditorApplication.update -= RefreshContainer;
        }

        private static void OnProcessorComplete(YVRConfigurationSummary summary)
        {
            int outstandingCount = 0;
            s_CurrentIcon.tooltip = summary.GetOutStandingString(ref s_TaskLevel, ref outstandingCount);
            s_CurrentIcon = s_NoIssueIcon;
            if (outstandingCount > 0)
            {
                s_CurrentIcon = s_TaskLevel switch
                {
                    YVRProjectSetup.TaskLevel.Required => s_CriticalIcon,
                    YVRProjectSetup.TaskLevel.Recommended => s_RecommendIcon,
                    YVRProjectSetup.TaskLevel.Optional => s_OptionalIcon,
                    _ => s_NoIssueIcon
                };
            }
        }

        private static void RefreshGUI()
        {
            if (s_IconStyle == null)
            {
                s_IconStyle = new GUIStyle("StatusBarIcon");
            }

            var screenWidth = EditorGUIUtility.GetMainWindowPosition().width;

            var settings = Lightmapping.GetLightingSettingsForScene(SceneManager.GetActiveScene());
            float value = settings == null || settings.bakedGI || settings.realtimeGI ? 130 : 104;

            var currentRect = new Rect(screenWidth - value, 0, 26, 30);
            GUILayout.BeginArea(currentRect);
            if (GUILayout.Button(s_CurrentIcon, s_IconStyle))
            {
                YVRProjectSetupEditorWindow.ShowYVRProjectSetupWindow();
            }
            //var buttonRect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(currentRect, MouseCursor.Link);

            GUILayout.EndArea();
        }
    }
}