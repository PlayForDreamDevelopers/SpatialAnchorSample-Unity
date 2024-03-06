using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace YVR.Core.Editor
{
    public class YVRProjectSetupEditorWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset editorWindowAsset;
        private List<YVRProjectSetupItem> itemList = new List<YVRProjectSetupItem>();

        private Button projectSetupButton;
        private Button performanceLintButton;
        private Button portingButton;
        private Button refreshButton;
        private Button feedbackButton;

        private YVRProjectSetup.TaskCategory currentTaskCategory = YVRProjectSetup.TaskCategory.ProjectSetup;
        private VisualElement root;

        [MenuItem("YVR/Project Setup Tool")]
        public static void ShowYVRProjectSetupWindow()
        {
            YVRProjectSetupEditorWindow wnd = GetWindow<YVRProjectSetupEditorWindow>();
            wnd.position = new Rect(0, 0, 1200, 800);
            wnd.titleContent = new GUIContent("YVRProjectSetup");
        }

        public void CreateGUI()
        {
            InitGUI();
        }

        private void InitGUI()
        {
            root = rootVisualElement;
            root.Clear();
            root.Add(editorWindowAsset.Instantiate());
            projectSetupButton = root.Q<Button>("ProjectSetupBtn");
            performanceLintButton = root.Q<Button>("PerformanceLintBtn");
            portingButton = root.Q<Button>("PortingButton");

            refreshButton = root.Q<Button>("RefreshButton");
            feedbackButton = root.Q<Button>("FeedbackButton");

            projectSetupButton.RegisterCallback<MouseUpEvent>(OnProjectSetupButtonClick);
            performanceLintButton.RegisterCallback<MouseUpEvent>(OnPerformanceLintButtonClick);
            portingButton.RegisterCallback<MouseUpEvent>(OnPortingButtonClick);
            refreshButton.RegisterCallback<MouseUpEvent>(OnRefreshButtonClick);
            feedbackButton.RegisterCallback<MouseUpEvent>(OnFeedbackButtonClick);

            OnProjectSetupButtonClick(null);
        }

        private void RefreshGUI(YVRProjectSetup.TaskCategory taskCategory)
        {
            currentTaskCategory = taskCategory;

            VisualElement requireItemContainer = root.Q<VisualElement>("RequireItemContainer");
            VisualElement notRequireItemContainer = root.Q<VisualElement>("NotRequireItemContainer");
            VisualElement fixedItemContainer = root.Q<VisualElement>("FixedItemContainer");
            VisualElement ignoreItemContainer = root.Q<VisualElement>("IgnoredItemContainer");

            requireItemContainer.Clear();
            notRequireItemContainer.Clear();
            fixedItemContainer.Clear();
            ignoreItemContainer.Clear();

            for (int i = 0; i < itemList.Count; i++)
            {
                YVRProjectSetupItem yvrProjectSteupItem = itemList[i];
                yvrProjectSteupItem.Release();
            }
            itemList.Clear();

            ShowTask(tasks => tasks.Where(task => task.taskCategory == taskCategory && !task.ignore && !task.isDone.Invoke() && task.taskLevel == YVRProjectSetup.TaskLevel.Required).ToList(), requireItemContainer);
            ShowTask(tasks => tasks.Where(task => task.taskCategory == taskCategory && !task.ignore && !task.isDone.Invoke() && task.taskLevel != YVRProjectSetup.TaskLevel.Required).OrderByDescending(task => task.taskLevel).ToList(), notRequireItemContainer);
            ShowTask(tasks => tasks.Where(task => task.taskCategory == taskCategory && !task.ignore && task.isDone.Invoke()).ToList(), fixedItemContainer);
            ShowTask(tasks => tasks.Where(task => task.taskCategory == taskCategory && task.ignore).ToList(), ignoreItemContainer);
        }

        private void ShowTask(Func<IEnumerable<YVRConfigurationTask>, List<YVRConfigurationTask>> filter, VisualElement root)
        {
            IEnumerable<YVRConfigurationTask> tasks = YVRProjectSetup.GetTasks();
            if (tasks == null) return;

            List<YVRConfigurationTask> filteredTasks = filter(tasks);

            for (int i = 0; i < filteredTasks.Count; i++)
            {
                YVRProjectSetupItem yvrProjectSetupItem = new YVRProjectSetupItem();
                yvrProjectSetupItem.Init(filteredTasks[i], OnFixButtonClick, OnSourceButtonClick, OnDocumentButtonClick, OnIgnoreButtonClick);
                root.Add(yvrProjectSetupItem.GetElement());
                itemList.Add(yvrProjectSetupItem);
            }
        }

        private void OnProjectSetupButtonClick(MouseUpEvent mouseUpEvent)
        {
            RefreshGUI(YVRProjectSetup.TaskCategory.ProjectSetup);
            SetCategoryButtonEnable();
            projectSetupButton.SetEnabled(false);
        }

        private void OnPerformanceLintButtonClick(MouseUpEvent mouseUpEvent)
        {
            RefreshGUI(YVRProjectSetup.TaskCategory.Performance);
            SetCategoryButtonEnable();
            performanceLintButton.SetEnabled(false);
        }

        private void OnPortingButtonClick(MouseUpEvent mouseUpEvent)
        {
            RefreshGUI(YVRProjectSetup.TaskCategory.Porting);
            SetCategoryButtonEnable();
            portingButton.SetEnabled(false);
        }

        private void OnRefreshButtonClick(MouseUpEvent mouseUpEvent)
        {
            RefreshGUI(currentTaskCategory);
        }

        private void OnFeedbackButtonClick(MouseUpEvent mouseUpEvent)
        {
            Application.OpenURL("https://www.wjx.cn/vm/w2M5NFN.aspx#");
        }

        private void SetCategoryButtonEnable()
        {
            projectSetupButton.SetEnabled(true);
            performanceLintButton.SetEnabled(true);
            portingButton.SetEnabled(true);
        }


        private void OnFixButtonClick(YVRConfigurationTask task)
        {
            YVRProjectSetup.FixTask(task, OnTaskFixed);
        }

        private void OnSourceButtonClick(YVRConfigurationTask task)
        {
            StackTrace stackTrace = task.stackTrace;
            StackFrame frame = stackTrace.GetFrame(1);
            var method = frame.GetMethod();
            IEnumerable<MethodInfo> methods = typeof(YVRProjectSetup).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(method => method.Name == "AddTask");
            if (methods.Contains(method))
            {
                frame = stackTrace.GetFrame(2);
            }
            string path = frame.GetFileName();
            int line = frame.GetFileLineNumber();
            path = path.Replace("\\", "/");
            if (path.StartsWith(Application.dataPath))
            {
                path = Path.Combine("Assets/", path.Substring(Application.dataPath.Length + 1));
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)), line);
            }
            else
            {

                System.Diagnostics.Process.Start(path);
            }
        }

        private void OnDocumentButtonClick(YVRConfigurationTask task)
        {
            Application.OpenURL(task.documentationUrl);
        }

        private void OnIgnoreButtonClick(YVRConfigurationTask task)
        {
            bool isIgnore = task.ignore;
            task.SetIgnore(!isIgnore);
            RefreshGUI(currentTaskCategory);
        }

        private void OnTaskFixed(YVRConfigurationTaskProcessor processor)
        {
            RefreshGUI(currentTaskCategory);
        }
    }
}
