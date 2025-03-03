using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YVR.Platform;

public class UIAchievementBitfieldOperation : MonoBehaviour, IAchievementOperation
{
    [SerializeField] private Text m_TxtProgress;
    [SerializeField] private Transform m_FieldItemParent;
    [SerializeField] private GameObject m_TogFieldItem;

    private List<Toggle> m_ToggleList = new();
    private int m_BitfieldLength;
    private int m_UnlockTarget;

    public Action<int[]> onUpdateBitfield;

    public void Initialize(AchievementDefinition definition)
    {
        m_BitfieldLength = definition.bitfieldLength;
        m_UnlockTarget = definition.target;

        int[] data = new int[m_BitfieldLength];
        for (int i = 0; i < m_BitfieldLength; i++)
            data[i] = 0;

        m_TxtProgress.text = $"Current progress: 0/{m_BitfieldLength}, reach {m_UnlockTarget} can unlock";

        Refresh(data);
    }

    public void UpdateProgress(AchievementProgress progress)
    {
        int[] data = new int[m_BitfieldLength];
        int bitfieldProgress = 0;

        for (int i = 0; i < data.Length; i++)
        {
            int value = progress.bitfieldProgress.Substring(i, 1) == "0" ? 0 : 1;
            data[i] = value;
            bitfieldProgress += value;
        }

        m_TxtProgress.text
            = $"Current progress: {bitfieldProgress}/{m_BitfieldLength}, reach {m_UnlockTarget} can unlock";

        Refresh(data);
    }

    private void Refresh(int[] bitfield)
    {
        ClearList();

        foreach (int value in bitfield)
        {
            var tog = Instantiate(m_TogFieldItem, m_FieldItemParent).GetComponent<Toggle>();
            tog.gameObject.SetActive(true);
            Transform togTransform = tog.transform;
            togTransform.localPosition = Vector3.zero;
            togTransform.localScale = Vector3.one;
            togTransform.localRotation = Quaternion.identity;

            tog.SetIsOnWithoutNotify(value == 1);
            tog.onValueChanged.AddListener(OnToggleValueChanged);

            m_ToggleList.Add(tog);
        }
    }

    private void ClearList()
    {
        for (int i = m_ToggleList.Count - 1; i >= 0; i--)
            DestroyImmediate(m_ToggleList[i].gameObject);

        m_ToggleList.Clear();
    }

    private void OnToggleValueChanged(bool isOn)
    {
        int[] data = new int[m_ToggleList.Count];

        for (int i = 0; i < m_ToggleList.Count; i++)
            data[i] = m_ToggleList[i].isOn ? 1 : 0;

        onUpdateBitfield?.Invoke(data);
    }
}