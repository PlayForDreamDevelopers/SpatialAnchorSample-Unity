using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YVR.Platform;

public class UIAchievementDisplay : MonoBehaviour
{
    [SerializeField] private Button m_BtnRefresh;
    [SerializeField] private Transform m_AchievementItemContent;
    [SerializeField] private GameObject m_AchievementItem;

    private List<UIAchievementItem> m_ItemList = new();

    private void Start()
    {
        m_BtnRefresh.onClick.AddListener(GetDefinition);

        GetDefinition();
    }

    private void GenerateAchievementList(AchievementDefinitionList data)
    {
        ClearList();

        foreach (AchievementDefinition definition in data)
        {
            var item = Instantiate(m_AchievementItem, m_AchievementItemContent).GetComponent<UIAchievementItem>();
            item.gameObject.SetActive(true);
            item.transform.SetParent(m_AchievementItemContent);
            item.Standardize();
            item.Initialize(definition, GetDefinition, GetProgress);

            m_ItemList.Add(item);
        }
    }

    private void UpdateDefinitions(AchievementDefinitionList data)
    {
        foreach (AchievementDefinition definition in data)
        {
            UIAchievementItem item = m_ItemList.Find(x => x.apiName == definition.apiName);
            item.UpdateDefinition(definition);
        }
    }

    private void UpdateProgress(AchievementProgressList data)
    {
        foreach (AchievementProgress progress in data)
            m_ItemList.Find(x => x.apiName == progress.apiName)?.UpdateProgress(progress);
    }

    private void ClearList()
    {
        for (int i = m_ItemList.Count - 1; i >= 0; i--)
            DestroyImmediate(m_ItemList[i].gameObject);

        m_ItemList.Clear();
    }

    #region Definition

    private void GetDefinition() { Achievement.GetAllDefinitions().OnComplete(GetAllDefinitionsCallback); }

    private void GetDefinition(string apiName)
    {
        Achievement.GetDefinitionByName(new[] {apiName}).OnComplete(GetDefinitionsCallback);
    }

    private void GetAllDefinitionsCallback(YVRMessage<AchievementDefinitionList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"UIAchievementDisplay.GetAllDefinitionsCallback: error -> {msg.error}");
            return;
        }

        GenerateAchievementList(msg.data);

        GetProgress();
    }

    private void GetDefinitionsCallback(YVRMessage<AchievementDefinitionList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"UIAchievementDisplay.GetDefinitionsCallback: error -> {msg.error}");
            return;
        }

        UpdateDefinitions(msg.data);
    }

    #endregion

    #region Progress

    private void GetProgress() { Achievement.GetAllProgress().OnComplete(GetProgressCallback); }

    private void GetProgress(string apiName)
    {
        Achievement.GetProgressByName(new[] {apiName}).OnComplete(GetProgressCallback);
    }

    private void GetProgressCallback(YVRMessage<AchievementProgressList> msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"UIAchievementDisplay.GetProgressCallback: error -> {msg.error}");
            return;
        }

        UpdateProgress(msg.data);
    }

    #endregion
}