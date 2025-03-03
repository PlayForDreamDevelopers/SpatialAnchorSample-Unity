using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DebugItem : MonoBehaviour
{
    [SerializeField] private Button m_Button;
    [SerializeField] private Image m_ImageBG;
    [SerializeField] private Image m_ImageType;
    [SerializeField] private Text m_TextLog;
    [SerializeField] private GameObject m_LogCollapseGroup;
    [SerializeField] private Text m_TextCollapseCount;
    [SerializeField] private Color m_ColorSingle;
    [SerializeField] private Color m_ColorDual;

    private DebugInfo m_Info;
    private int m_Index;

    public void Initialize(DebugInfo info, int index, Sprite typeIcon, Action<DebugInfo> onClick)
    {
        m_Info = info;
        m_Index = index;

        m_ImageType.sprite = typeIcon;

        SetContent();

        m_Button.onClick.RemoveAllListeners();
        m_Button.onClick.AddListener(() => onClick(m_Info));
    }

    private void SetContent()
    {
        StringBuilder sb = DebugManager.instance.sharedStringBuilder;
        sb.Length = 0;

        m_Info.timeStamp.AppendTime(sb);
        sb.Append(" ").Append(m_Info.logString);

        m_TextLog.text = sb.ToString();

        if (DebugManager.instance.collapseLogs && m_Info.collapseCount > 0)
            ShowCount();
        else
            HideCount();

        m_ImageBG.color = m_Index % 2 == 0 ? m_ColorDual : m_ColorSingle;
    }

    public void ShowCount()
    {
        m_LogCollapseGroup.SetActive(true);
        m_TextCollapseCount.text = m_Info.collapseCount.ToString();
    }

    public void HideCount() { m_LogCollapseGroup.SetActive(false); }
}