using System;
using UnityEngine;
using UnityEngine.UI;
using YVR.Platform;

public class UIAchievementSimpleOperation : MonoBehaviour, IAchievementOperation
{
    [SerializeField] private Button m_BtnUnlock;

    public Action onDirectUnlock;

    public void Initialize(AchievementDefinition definition)
    {
        m_BtnUnlock.onClick.AddListener(() =>
        {
            onDirectUnlock?.Invoke();
        });
    }

    public void UpdateProgress(AchievementProgress progress) {  }
}