using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace YVR.Core.Editor
{
    public class YVRProjectSetupItem
    {
        private YVRConfigurationTask m_Task;
        private Button m_FixBtn;
        private Button m_SourceBtn;
        private Button m_DocumentBtn;
        private Button m_IgnoreBtn;
        private Label m_TaskName;
        private VisualElement m_ItemLevelIcon;
        private VisualElement rootElement;

        private Action<YVRConfigurationTask> onFixedBtnClick;
        private Action<YVRConfigurationTask> onSourceBtnClick;
        private Action<YVRConfigurationTask> onDocumentBtnClick;
        private Action<YVRConfigurationTask> onIgnoreBtnClick;

        public void Init(YVRConfigurationTask task, Action<YVRConfigurationTask> onFixedBtnClick, Action<YVRConfigurationTask> onSourceBtnClick, Action<YVRConfigurationTask> onDocumentBtnClick, Action<YVRConfigurationTask> onIgnoreBtnClick)
        {
            this.m_Task = task;
            rootElement = YVRProjectSetupItemPool.instance.Get();
            m_FixBtn = rootElement.Q<Button>("FixBtn");
            m_SourceBtn = rootElement.Q<Button>("SourceBtn");
            m_DocumentBtn = rootElement.Q<Button>("DocumentBtn");
            m_IgnoreBtn = rootElement.Q<Button>("IgnoreBtn");
            m_TaskName = rootElement.Q<Label>("IssueMessage");
            m_ItemLevelIcon = rootElement.Q<VisualElement>("ItemLevelIcon");

            bool isDone = task.isDone.Invoke();
            m_TaskName.text = isDone ? task.fixedMessage : task.issueMessage;
            m_FixBtn.SetEnabled(!isDone && task.fixAction != null);
            m_FixBtn.pickingMode = !isDone && task.fixAction != null ? PickingMode.Position : PickingMode.Ignore;
            m_DocumentBtn.SetEnabled(task.documentationUrl != null);
            m_DocumentBtn.pickingMode = task.documentationUrl != null ? PickingMode.Position : PickingMode.Ignore;
            m_ItemLevelIcon.style.backgroundImage = GetLevelIcon();
            m_IgnoreBtn.tooltip = task.ignore ? "unignore" : "ignore";

            m_FixBtn.RegisterCallback<MouseUpEvent>(OnFixButtonClick);
            m_SourceBtn.RegisterCallback<MouseUpEvent>(OnSourceButtonClick);
            m_DocumentBtn.RegisterCallback<MouseUpEvent>(OnDocumentButtonClick);
            m_IgnoreBtn.RegisterCallback<MouseUpEvent>(OnIgnoreButtonClick);

            this.onFixedBtnClick = onFixedBtnClick;
            this.onSourceBtnClick = onSourceBtnClick;
            this.onDocumentBtnClick = onDocumentBtnClick;
            this.onIgnoreBtnClick = onIgnoreBtnClick;
        }

        public VisualElement GetElement()
        {
            if (rootElement == null)
            {
                Debug.LogError("rootElement not exist please call Init first");
            }
            return rootElement;
        }

        public void Release()
        {
            m_FixBtn.UnregisterCallback<MouseUpEvent>(OnFixButtonClick);
            m_SourceBtn.UnregisterCallback<MouseUpEvent>(OnSourceButtonClick);
            m_DocumentBtn.UnregisterCallback<MouseUpEvent>(OnDocumentButtonClick);
            m_IgnoreBtn.UnregisterCallback<MouseUpEvent>(OnIgnoreButtonClick);
            YVRProjectSetupItemPool.instance.Return(rootElement);
        }

        private Texture2D GetLevelIcon()
        {
            if (m_Task.isDone.Invoke())
            {
                return YVRProjectSetupResourcesLoader.instance.LoadTexture<Texture2D>("ProjectSetup_Fixed.png");
            }

            switch (m_Task.taskLevel)
            {
                case YVRProjectSetup.TaskLevel.Required:
                    return YVRProjectSetupResourcesLoader.instance.LoadTexture<Texture2D>("ProjectSetup_Required.png");
                case YVRProjectSetup.TaskLevel.Recommended:
                    return YVRProjectSetupResourcesLoader.instance.LoadTexture<Texture2D>("ProjectSetup_Recommended.png");
                case YVRProjectSetup.TaskLevel.Optional:
                    return YVRProjectSetupResourcesLoader.instance.LoadTexture<Texture2D>("ProjectSetup_Optional.png");
                default: break;
            }

            return YVRProjectSetupResourcesLoader.instance.LoadTexture<Texture2D>("ProjectSetup_Optional.png");
        }

        private void OnFixButtonClick(MouseUpEvent mouseUpEvent)
        {
            onFixedBtnClick?.Invoke(m_Task);
        }

        private void OnSourceButtonClick(MouseUpEvent mouseUpEvent)
        {
            onSourceBtnClick?.Invoke(m_Task);
        }

        private void OnDocumentButtonClick(MouseUpEvent mouseUpEvent)
        {
            onDocumentBtnClick?.Invoke(m_Task);
        }

        private void OnIgnoreButtonClick(MouseUpEvent mouseUpEvent)
        {
            onIgnoreBtnClick?.Invoke(m_Task);
        }
    }
}
