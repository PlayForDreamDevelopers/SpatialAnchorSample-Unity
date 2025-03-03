using System;
using UnityEngine;
using UnityEngine.UI;

public class InputOption : MonoBehaviour
{
    [SerializeField] private InputOptionType m_InputOptionType;
    [SerializeField] private InputField m_InputField;
    [SerializeField] private Toggle m_Toggle;
    [SerializeField] private Dropdown m_Dropdown;

    public InputOptionType inputOptionType => m_InputOptionType;

    public string inputValue
    {
        get
        {
            switch (m_InputOptionType)
            {
                case InputOptionType.AppID:
                case InputOptionType.FriendAccountID:
                case InputOptionType.ProductSKU:
                case InputOptionType.AmountOfPayment:
                case InputOptionType.AchievementName:
                case InputOptionType.AddCount:
                case InputOptionType.AddField:
                case InputOptionType.CurrentPage:
                case InputOptionType.PageSize:
                case InputOptionType.LeaderboardName:
                case InputOptionType.CurrentStart:
                case InputOptionType.Score:
                case InputOptionType.ExtraData:
                case InputOptionType.BeginTimeFormat:
                case InputOptionType.EndTimeFormat:
                    return m_InputField.text;
                case InputOptionType.QuitAppWhenEntitlementCheckFail:
                    return m_Toggle.isOn.ToString();
                case InputOptionType.PageType:
                case InputOptionType.DataDirection:
                case InputOptionType.UpdatePolicy:
                    return m_Dropdown.options[m_Dropdown.value].text;
                default:
                    return string.Empty;
            }
        }
    }

    private void Start()
    {
        m_InputField.gameObject.SetActive(CommonDefine.inputFieldTypesMap.Contains(m_InputOptionType));
        m_Toggle.gameObject.SetActive(CommonDefine.toggleTypesMap.Contains(m_InputOptionType));
        m_Dropdown.gameObject.SetActive(CommonDefine.dropdownTypesMap.Contains(m_InputOptionType));
    }

    public void SetDefaultData(string defaultData)
    {
        m_InputField.SetTextWithoutNotify(defaultData);
    }

    public void SetDefaultData(bool defaultData)
    {
        m_Toggle.SetIsOnWithoutNotify(defaultData);
    }

    public void SetDefaultData(int defaultDataIndex)
    {
        m_Dropdown.SetValueWithoutNotify(defaultDataIndex);
    }
}