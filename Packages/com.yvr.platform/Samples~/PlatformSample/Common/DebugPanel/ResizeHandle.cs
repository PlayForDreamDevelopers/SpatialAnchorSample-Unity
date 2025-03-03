using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeHandle : MonoBehaviour, IDragHandler
{
    [SerializeField] private Vector2 m_DragRange;

    public Action<float> onHandleDrag;

    public void OnDrag(PointerEventData eventData)
    {
        float fixedY = Mathf.Clamp(eventData.position.y, m_DragRange.x, m_DragRange.y);

        onHandleDrag?.Invoke(fixedY);
    }
}
