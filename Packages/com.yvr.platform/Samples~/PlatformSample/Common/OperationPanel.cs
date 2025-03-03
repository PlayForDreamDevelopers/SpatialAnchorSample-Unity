using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OperationPanel : MonoBehaviour
{
    [SerializeField] private List<OperationToggle> m_OperationToggles;
    [SerializeField] private List<OperationPage> m_OperationPages;
    [SerializeField] private int m_DefaultIndex;
    [SerializeField] private Button m_ButtonBackToLobby;
    [SerializeField] private Toggle m_ShowUI;
    [SerializeField] private GameObject m_DisplayUI;

    private void Start()
    {
        m_ButtonBackToLobby?.onClick.AddListener(OnClickBackToLobby);

        foreach (var toggle in m_OperationToggles)
            toggle.onValueChanged += OnToggleValueChanged;

        if (m_OperationToggles == null || m_OperationPages == null)
            return;

        if (m_DefaultIndex >= m_OperationToggles.Count)
            return;

        OnToggleValueChanged(m_OperationToggles[m_DefaultIndex].toggleType);

        m_ShowUI?.onValueChanged.AddListener(OnShowUIToggleValueChanged);
    }

    private void OnClickBackToLobby() { SceneManager.LoadSceneAsync("Entrance"); }

    private void OnToggleValueChanged(OperationType type)
    {
        m_OperationToggles.ForEach(toggle => toggle.SetToggle(toggle.toggleType == type));
        m_OperationPages.ForEach(page => page.SetPage(page.operationType == type));
    }

    private void OnShowUIToggleValueChanged(bool isOn)
    {
        m_DisplayUI?.SetActive(isOn);
    }
}