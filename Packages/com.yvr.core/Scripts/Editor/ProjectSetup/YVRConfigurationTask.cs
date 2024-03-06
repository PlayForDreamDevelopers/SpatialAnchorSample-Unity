using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace YVR.Core.Editor
{
    public class YVRConfigurationTask
    {
        public Hash128 Uid { get; }
        public YVRProjectSetup.TaskGroup taskGroup { get; }
        public YVRProjectSetup.TaskLevel taskLevel { get; }

        public YVRProjectSetup.TaskCategory taskCategory { get; }

        public Func<bool> isDone { get; }
        public Action fixAction { get; }
        public string issueMessage { get; }
        public string fixedMessage { get; }
        public string documentationUrl { get; }
        public bool ignore { get; set; }

        public StackTrace stackTrace { get; }

        public YVRConfigurationTask(YVRProjectSetup.TaskGroup taskGroup, YVRProjectSetup.TaskLevel taskLevel, YVRProjectSetup.TaskCategory taskCategory, Func<bool> isDone, Action fixAction, string issueMessage, string fixedMessage, string documentationUrl)
        {
            stackTrace = new StackTrace(true);

            this.taskGroup = taskGroup;
            this.taskLevel = taskLevel;
            this.isDone = isDone;
            this.fixAction = fixAction;
            this.issueMessage = issueMessage;
            this.fixedMessage = fixedMessage;
            this.documentationUrl = documentationUrl;
            this.taskCategory = taskCategory;

            var hash = new Hash128();
            hash.Append(issueMessage);
            Uid = hash;

            ignore = GetIgnore();
        }

        public void Fix()
        {
            try
            {
                fixAction();
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError($"fix task fail {exception}");
            }
        }

        public void SetIgnore(bool ignore)
        {
            EditorPrefs.SetBool(typeof(YVRProjectSetup).Name + Uid.ToString(), ignore);
            this.ignore = ignore;
        }

        private bool GetIgnore()
        {
            return EditorPrefs.GetBool(typeof(YVRProjectSetup).Name + Uid.ToString(), false);
        }
    }
}
