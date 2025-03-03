using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationPage : MonoBehaviour
{
    [SerializeField] public OperationType operationType;
    [SerializeField] protected Button m_ButtonExecute;
    [SerializeField] protected List<InputOption> m_InputOptions;
    [SerializeField] protected Text m_TextResult;

    protected virtual void Start() { m_ButtonExecute?.onClick.AddListener(OnClickExecute); }

    protected virtual void OnClickExecute() { Debug.Log($"OperationPage.OnClickExecute: pageType -> {operationType}"); }

    protected string GetInputValueByType(InputOptionType inputOptionType)
    {
        if (m_InputOptions == null)
            return string.Empty;

        InputOption option = m_InputOptions.Find(x => x.inputOptionType == inputOptionType);

        return option?.inputValue;
    }

    protected InputOption GetInputOption(InputOptionType inputOptionType)
    {
        if (m_InputOptions == null)
            return null;

        return m_InputOptions.Find(x => x.inputOptionType == inputOptionType);
    }

    public void SetPage(bool isOn) { gameObject.SetActive(isOn); }
}