using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YVR.Platform;
using YVR.Utilities;

public class UIAchievementItem : MonoBehaviour
{
    #region Definition

    [SerializeField] private Image m_ImgIcon;
    [SerializeField] private Text m_TxtStatus;
    [SerializeField] private Text m_TxtName;
    [SerializeField] private Text m_TxtType;
    [SerializeField] private Text m_TxtDescription;
    [SerializeField] private Text m_TxtUnlockDescription;
    [SerializeField] private Toggle m_TogRefreshAfterOperate;

    #endregion

    #region Operation

    [SerializeField] private Button m_BtnGetDefinition;
    [SerializeField] private Button m_BtnGetProgress;
    [SerializeField] private UIAchievementSimpleOperation m_SimpleOperation;
    [SerializeField] private UIAchievementCountOperation m_CountOperation;
    [SerializeField] private UIAchievementBitfieldOperation m_BitfieldOperation;
    [SerializeField] private GameObject m_UnlockedTips;

    #endregion

    private AchievementDefinition m_Definition;
    private AchievementProgress m_Progress;
    private UnityWebRequest m_UnityWebRequest;
    private Action<string> m_GetDefinition;
    private Action<string> m_GetProgress;
    public string apiName => m_Definition?.apiName;

    public void Initialize(AchievementDefinition definition, Action<string> onGetDefinition,
                           Action<string> onGetProgress)
    {
        m_GetDefinition = onGetDefinition;
        m_GetProgress = onGetProgress;

        m_SimpleOperation.Initialize(definition);
        m_SimpleOperation.onDirectUnlock += Unlock;

        m_CountOperation.Initialize(definition);
        m_CountOperation.onAddCount += AddCount;

        m_BitfieldOperation.Initialize(definition);
        m_BitfieldOperation.onUpdateBitfield += UpdateBitfield;

        m_BtnGetDefinition.onClick.AddListener(() =>
        {
            m_GetDefinition?.Invoke(apiName);
        });

        m_BtnGetProgress.onClick.AddListener(() =>
        {
            m_GetProgress?.Invoke(apiName);
        });

        UpdateDefinition(definition);

        m_TxtStatus.text = "locked";

        m_SimpleOperation.gameObject.SetActive(m_Definition.type == 0);
        m_CountOperation.gameObject.SetActive(m_Definition.type == 1);
        m_BitfieldOperation.gameObject.SetActive(m_Definition.type == 2);
        m_UnlockedTips.SetActive(false);
    }

    public void Standardize()
    {
        Transform self = transform;
        self.localPosition = Vector3.zero;
        self.localRotation = Quaternion.identity;
        self.localScale = Vector3.one;
    }

    public void UpdateDefinition(AchievementDefinition definition)
    {
        m_Definition = definition;

        m_TxtName.text = m_Definition.title;
        m_TxtType.text = CommonDefine.typeDefine[m_Definition.type];
        m_TxtDescription.text = m_Definition.description;
        m_TxtUnlockDescription.text = m_Definition.unlockedDescription;

        StartCoroutine(LoadIcon());
    }

    public void UpdateProgress(AchievementProgress progress)
    {
        m_Progress = progress;

        m_TxtStatus.text = m_Progress.isUnlocked ? "Unlocked" : "locked";
        m_UnlockedTips.SetActive(progress.isUnlocked);

        m_SimpleOperation.UpdateProgress(m_Progress);
        m_CountOperation.UpdateProgress(m_Progress);
        m_BitfieldOperation.UpdateProgress(m_Progress);

        StartCoroutine(LoadIcon());
    }

    private IEnumerator LoadIcon()
    {
        string url = m_Progress != null
            ? m_Progress.isUnlocked ? m_Definition.unlockedImageFile : m_Definition.lockedImageFile
            : m_Definition.lockedImageFile;

        m_UnityWebRequest = UnityWebRequestTexture.GetTexture(url);
        yield return m_UnityWebRequest.SendWebRequest();
        if (m_UnityWebRequest.error != null)
        {
            Debug.Log($"Requesting... url: {url}");
            yield return m_UnityWebRequest;
        }

        DownloadHandler downloadHandler = m_UnityWebRequest.downloadHandler;

        if (!downloadHandler.isDone)
        {
            Debug.Log($"Downloading... url: {url}");
            yield return downloadHandler;
        }
        else
        {
            Debug.Log($"Download finish! url: {url}");

            var texture2D = new Texture2D(128, 128);
            yield return texture2D.LoadImage(downloadHandler.data);

            m_ImgIcon.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                                             new Vector2(0, 0));
        }

        yield return null;
    }

    private void Unlock() { Achievement.UnlockAchievement(apiName).OnComplete(UnlockCallback); }

    private void UnlockCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"UIAchievementItem.UnlockCallback: error -> {msg.error}");
            return;
        }

        if (m_TogRefreshAfterOperate.isOn)
            m_GetProgress?.Invoke(apiName);
    }

    private void AddCount(int num) { Achievement.AchievementAddCount(apiName, num).OnComplete(AddCountCallback); }

    private void AddCountCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"UIAchievementItem.AddCountCallback: error -> {msg.error}");
            return;
        }

        if (m_TogRefreshAfterOperate.isOn)
            m_GetProgress?.Invoke(apiName);
    }

    private void UpdateBitfield(int[] data)
    {
        string bitfieldData = String.Empty;
        data.ForEach<int>(x => bitfieldData += x.ToString());
        Achievement.AchievementAddFields(apiName, bitfieldData).OnComplete(AddFieldsCallback);
    }

    private void AddFieldsCallback(YVRMessage msg)
    {
        if (msg.isError)
        {
            Debug.LogError($"UIAchievementItem.AddFieldsCallback: error -> {msg.error}");
            return;
        }

        if (m_TogRefreshAfterOperate.isOn)
            m_GetProgress?.Invoke(apiName);
    }

    private void OnDestroy()
    {
        StopCoroutine(LoadIcon());
        m_UnityWebRequest.Abort();
        m_Definition = null;
        m_Progress = null;
    }
}