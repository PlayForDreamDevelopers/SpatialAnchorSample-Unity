using System;
using UnityEngine;
using UnityEngine.UI;
using YVR.Platform;

public class UIAchievementCountOperation : MonoBehaviour, IAchievementOperation
{
    [SerializeField] private Text m_TxtProgress;
    [SerializeField] private InputField m_InputAddNum;
    [SerializeField] private Button m_BtnAdd;

    public Action<int> onAddCount;

    public void Initialize(AchievementDefinition definition)
    {
        m_BtnAdd.onClick.AddListener(() =>
        {
            if (string.IsNullOrWhiteSpace(m_InputAddNum.text))
                return;

            int num = int.Parse(m_InputAddNum.text);
            onAddCount?.Invoke(num);
        });

        m_TxtProgress.text = $"Current progress: {0} / {definition.target}";
    }

    public void UpdateProgress(AchievementProgress progress)
    {
        m_TxtProgress.text = $"Current progress: {progress.countProgress} / {progress.target}";
    }
}