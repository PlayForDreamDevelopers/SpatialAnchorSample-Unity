using System;
using UnityEngine;
using UnityEngine.UI;

public class OperationToggle : MonoBehaviour
{
    [SerializeField] private OperationType m_OperationType;
    [SerializeField] private Button m_Button;
    [SerializeField] private Text m_TextOperationType;
    [SerializeField] private GameObject m_Mark;

    public OperationType toggleType => m_OperationType;

    public Action<OperationType> onValueChanged;

    private void Start()
    {
        m_TextOperationType.text = m_OperationType.ToString();

        m_Button.onClick.AddListener(() => onValueChanged?.Invoke(m_OperationType));
    }

    public void SetToggle(bool isOn) { m_Mark.SetActive(isOn); }
}