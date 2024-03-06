using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace YVR.Utilities.Editor
{
    internal sealed class LogInterceptor
    {
        private static LogInterceptor s_Current;
        private static LogInterceptor current => s_Current ??= new LogInterceptor();

        private FieldInfo m_ActiveTextInfo;
        private FieldInfo m_ConsoleWindowInfo;
        private MethodInfo m_SetActiveEntry;
        private object[] m_SetActiveEntryArgs;
        private object m_ConsoleWindow;
        private object consoleWindow => m_ConsoleWindow ??= m_ConsoleWindowInfo.GetValue(null);

        private List<string> m_IgnoreScripts = new()
            {"YVRLog.cs", "YLogLogger.cs", "YLogEditorAdapter.cs,", "LoggerBase.cs"};

        private LogInterceptor()
        {
            var consoleWindowType = Type.GetType("UnityEditor.ConsoleWindow,UnityEditor");

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            m_ActiveTextInfo = consoleWindowType.GetField("m_ActiveText", bindingFlags);
            m_SetActiveEntry = consoleWindowType.GetMethod("SetActiveEntry", bindingFlags);
            m_ConsoleWindowInfo
                = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            m_SetActiveEntryArgs = new object[] {null};
        }

        [OnOpenAsset]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            UnityEngine.Object instance = EditorUtility.InstanceIDToObject(instanceID);
            return AssetDatabase.GetAssetOrScenePath(instance).EndsWith(".cs") && current.TryOpenAsset();
        }

        private bool TryOpenAsset()
        {
            string stackTrace = GetStackTrace();
            if (string.IsNullOrEmpty(stackTrace)) return false;

            var paths = stackTrace.Split("\n")
                                  .Where(line => line.Contains(" (at ") && !m_IgnoreScripts.Any(line.Contains));
            return paths.Count() > 1 && OpenScriptAsset(paths.ElementAt(1));

            string GetStackTrace()
            {
                if (consoleWindow == null || consoleWindow != EditorWindow.focusedWindow as object) return "";
                object value = m_ActiveTextInfo.GetValue(consoleWindow);
                return value != null ? value.ToString() : "";
            }

            bool OpenScriptAsset(string path)
            {
                int startIndex = path.IndexOf(" (at ", StringComparison.Ordinal) + 5;
                int endIndex = path.IndexOf(".cs:", StringComparison.Ordinal) + 3;
                string filePath = path.Substring(startIndex, endIndex - startIndex);
                string lineStr = path.Substring(endIndex + 1, path.Length - endIndex - 2);
                TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);

                if (asset == null || !int.TryParse(lineStr, out int line)) return false;
                m_SetActiveEntry.Invoke(consoleWindow, m_SetActiveEntryArgs);
                EditorGUIUtility.PingObject(asset);
                AssetDatabase.OpenAsset(asset, line);
                return true;
            }
        }
    }
}