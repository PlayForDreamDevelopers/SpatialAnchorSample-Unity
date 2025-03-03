using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DebugScrollView : ScrollRect
{
    public delegate void ListItemRenderer(int index, GameObject item);

    public ListItemRenderer itemRenderer;

    private int m_NumItems;

    [Header("Item Prefab")] public RectTransform item;
    [Tooltip("Item Size")] public Vector2 itemSize;
    [Tooltip("Item Spacing")] public float spacing;
    [Tooltip("Padding")] public RectOffset padding;
    [Tooltip("Bottom Threshold")] public float bottomThreshold;

    private List<ItemInfo> m_VirtualItems = new();

    private class ItemInfo
    {
        public int index;
        public RectTransform item;
    }

    protected override void Awake()
    {
        base.Awake();

        horizontal = false;
    }

    private void ListenerChange(Vector2 changeV) { RefreshItemInfo(); }

    private void CalculateCount()
    {
        content.anchorMin = new Vector2(0, 1);
        content.anchorMax = new Vector2(1, 1);
        // content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
        float contentHeight = itemSize.y * m_NumItems + (m_NumItems - 1) * spacing + padding.top + padding.bottom;
        content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);

        item.pivot = new Vector2(0.5f, 1);
        item.anchorMin = new Vector2(0.5f, 1);
        item.anchorMax = new Vector2(0.5f, 1);

        float height = GetComponent<RectTransform>().rect.height;
        height -= (padding.top + padding.bottom);

        int leastItemCount = Mathf.CeilToInt(height / (itemSize.y + spacing));
        leastItemCount += 2;
        leastItemCount = leastItemCount > m_NumItems ? m_NumItems : leastItemCount;
        int maxNum = leastItemCount - m_VirtualItems.Count;

        if (maxNum > 0)
        {
            for (int i = m_VirtualItems.Count; i < leastItemCount; i++)
            {
                GameObject item = Instantiate(this.item.gameObject, content);
                ItemInfo itemInfo = new ItemInfo();
                itemInfo.item = item.GetComponent<RectTransform>();
                itemInfo.index = i;
                m_VirtualItems.Add(itemInfo);
                itemInfo.item.gameObject.SetActive(false);
                itemInfo.item.localScale = Vector3.one;
                itemInfo.item.sizeDelta = itemSize;
            }
        }
        else
        {
            maxNum = -maxNum;
            for (int i = 0; i < maxNum; i++)
            {
                GameObject go = m_VirtualItems[^1].item.gameObject; // ^1 means last index
                m_VirtualItems.RemoveAt(m_VirtualItems.Count - 1);
                Destroy(go);
            }
        }

        onValueChanged.RemoveAllListeners();
        onValueChanged.AddListener(ListenerChange);
    }

    public int numItems
    {
        get => m_NumItems;
        set
        {
            if (padding == null)
                throw new System.Exception("VirtualScroll: Set Padding first!");

            if (itemRenderer == null)
                throw new System.Exception("VirtualScroll: Set itemRenderer first!");

            m_NumItems = value;
            CalculateCount();
            RefreshList();
        }
    }

    public bool atBottom => Mathf.Abs(content.anchoredPosition.y + viewport.rect.height - content.sizeDelta.y) <
                            bottomThreshold;

    private void RefreshItem()
    {
        foreach (ItemInfo itemInfo in m_VirtualItems)
        {
            if (!itemInfo.item.gameObject.activeSelf)
            {
                itemInfo.item.gameObject.SetActive(true);
            }

            CalibrationPos(itemInfo);
            itemRenderer?.Invoke(itemInfo.index, itemInfo.item.gameObject);
        }
    }

    private void CalibrationPos(ItemInfo itemInfo)
    {
        float oldPos = itemInfo.item.anchoredPosition.y;
        float y = padding.top + itemInfo.index * itemSize.y + itemInfo.index * spacing;
        y *= -1;
        if (!Mathf.Approximately(oldPos, y))
            itemInfo.item.anchoredPosition = new Vector2(itemInfo.item.anchoredPosition.x, y);
    }

    private void RefreshItemInfo()
    {
        if (m_VirtualItems.Count == 0)
            return;

        float contentY = content.anchoredPosition.y;
        contentY -= padding.top;

        int index = (int) Mathf.Floor(contentY / (itemSize.y + spacing));
        index = index < 0 ? 0 : index;
        int virtualIndex = m_VirtualItems[0].index;

        if (index == virtualIndex)
            return;

        if (index < virtualIndex)
        {
            for (int i = 0; i < m_VirtualItems.Count; i++)
            {
                virtualIndex -= 1;
                ItemInfo itemInfo = m_VirtualItems[^1];
                m_VirtualItems.RemoveAt(m_VirtualItems.Count - 1);
                m_VirtualItems.Insert(0, itemInfo);
                if (virtualIndex == index)
                    break;
            }
        }
        else
        {
            for (int i = 0; i < m_VirtualItems.Count; i++)
            {
                virtualIndex += 1;
                ItemInfo itemInfo = m_VirtualItems[0];
                m_VirtualItems.RemoveAt(0);
                m_VirtualItems.Add(itemInfo);

                if (virtualIndex == index)
                    break;
            }
        }

        float startY = (padding.top + index * (itemSize.y + spacing)) * -1;
        int itemIndex = 0;

        for (int i = index; i < m_NumItems; i++)
        {
            if (m_VirtualItems.Count <= itemIndex)
                break;

            if (!m_VirtualItems[itemIndex].item.gameObject.activeSelf)
                m_VirtualItems[itemIndex].item.gameObject.SetActive(true);

            if (m_VirtualItems[itemIndex].index != i)
            {
                m_VirtualItems[itemIndex].index = i;
                m_VirtualItems[itemIndex].item.anchoredPosition
                    = new Vector2(m_VirtualItems[itemIndex].item.anchoredPosition.x, startY);
                itemRenderer?.Invoke(i, m_VirtualItems[itemIndex].item.gameObject);
            }

            startY -= itemSize.y + spacing;
            itemIndex++;
        }
    }

    public void CalibrationRefresh() { RefreshItemInfo(); }

    public GameObject GetListAt(int index)
    {
        ItemInfo itemInfo = m_VirtualItems.Find(item => (item.index == index));

        if (itemInfo != null)
            return itemInfo.item.gameObject;

        return null;
    }

    public void RefreshList() { RefreshItem(); }

    public void RefreshListIndex(int index)
    {
        ItemInfo itemInfo = m_VirtualItems.Find(item => (item.index == index));
        if (itemInfo == null)
            throw new System.Exception("RefreshListIndex: index out:" + index);
        else
            itemRenderer?.Invoke(index, itemInfo.item.gameObject);
    }

    public void ScrollToView(ScrollTo scrollTo)
    {
        if (scrollTo == ScrollTo.First)
            ScrollToView(0);
        else if (scrollTo == ScrollTo.Last)
            ScrollToView(m_NumItems - 1);
        else
            throw new System.Exception("ScrollToView: ScrollTo is null!");
    }

    public void ScrollToView(int index)
    {
        if (index < 0)
            return;

        if (index > (m_NumItems - 1))
            return;

        float height = (itemSize.y + spacing) * index;

        float maxHeight = content.rect.height - viewport.rect.height - padding.bottom;

        height = height > maxHeight ? maxHeight : height;

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, height);
    }
}

public enum ScrollTo
{
    First,
    Last
}