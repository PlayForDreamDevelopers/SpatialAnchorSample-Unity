using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_CanvasGroup;

    [SerializeField] private RectTransform m_RectTransformDebugs;
    [SerializeField] private RectTransform m_RectTransformStackTrace;

    [SerializeField] private Button m_ButtonClear;
    [SerializeField] private Button m_ButtonClose;
    [SerializeField] private Button m_ButtonMoveToBottom;

    [SerializeField] private Toggle m_ToggleCollapse;
    [SerializeField] private Toggle m_ToggleInfo;
    [SerializeField] private Toggle m_ToggleWarning;
    [SerializeField] private Toggle m_ToggleError;

    [SerializeField] private Text m_TextStackTrace;

    [SerializeField] private InputField m_InputSearch;

    [SerializeField] private DebugScrollView m_ScrollView;

    [SerializeField] private ResizeHandle m_ResizeHandle;

    [SerializeField] private Sprite m_IconInfo;
    [SerializeField] private Sprite m_IconWarning;
    [SerializeField] private Sprite m_IconError;

    private List<DebugInfo> m_DataCache;

    private void Start()
    {
        m_ScrollView.itemRenderer = ListItemRenderer;

        m_ResizeHandle.onHandleDrag += OnHandleDrag;

        DebugManager.instance.onDebugInfosChanged += OnDebugInfosChanged;

        m_ButtonClear.onClick.AddListener(DebugManager.instance.ClearDebug);
        m_ButtonClose.onClick.AddListener(() =>
        {
            if (m_CanvasGroup != null) m_CanvasGroup.alpha = 0;
        });

        m_ButtonMoveToBottom.onClick.AddListener(() => m_ScrollView.ScrollToView(ScrollTo.Last));

        m_ToggleCollapse.isOn = DebugManager.instance.collapseLogs;
        m_ToggleCollapse.onValueChanged.AddListener(isOn => DebugManager.instance.collapseLogs = isOn);

        m_ToggleInfo.isOn = DebugManager.instance.showInfo;
        m_ToggleInfo.onValueChanged.AddListener(isOn => DebugManager.instance.showInfo = isOn);

        m_ToggleWarning.isOn = DebugManager.instance.showWarning;
        m_ToggleWarning.onValueChanged.AddListener(isOn => DebugManager.instance.showWarning = isOn);

        m_ToggleError.isOn = DebugManager.instance.showError;
        m_ToggleError.onValueChanged.AddListener(isOn => DebugManager.instance.showError = isOn);
    }

    private void LateUpdate()
    {
        if (Time.frameCount % 3 == 0)
            m_ButtonMoveToBottom.gameObject.SetActive(!m_ScrollView.atBottom);
    }

    private void ListItemRenderer(int index, GameObject item)
    {
        DebugItem debugItem = item.GetComponent<DebugItem>();
        DebugInfo info = index >= m_DataCache.Count ? null : m_DataCache[index];
        if (info != null && debugItem)
            debugItem.Initialize(info, index, GetSpriteByType(m_DataCache[index].logType), OnClickDebugItem);
    }

    private Sprite GetSpriteByType(LogType type)
    {
        switch (type)
        {
            case LogType.Log:
                return m_IconInfo;
            case LogType.Warning:
                return m_IconWarning;
            case LogType.Error:
            case LogType.Exception:
                return m_IconError;
            default:
                return m_IconInfo;
        }
    }

    private void OnHandleDrag(float y)
    {
        m_RectTransformDebugs.offsetMin = new Vector2(0, y);
        m_RectTransformStackTrace.sizeDelta = new Vector2(m_RectTransformStackTrace.sizeDelta.x, y);
    }

    private void OnDebugInfosChanged(List<DebugInfo> infoList)
    {
        bool atBottom = m_ScrollView.atBottom;
        m_DataCache = infoList;
        m_ScrollView.numItems = infoList.Count;
        m_ScrollView.RefreshList();

        if (atBottom)
            m_ScrollView.ScrollToView(ScrollTo.Last);
    }

    private void OnClickDebugItem(DebugInfo info) { m_TextStackTrace.text = info.ToString(); }
}