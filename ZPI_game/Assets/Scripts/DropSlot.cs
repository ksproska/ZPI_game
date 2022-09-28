using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public int expectedId;
    private int _placedId;

    public void Start()
    {
        Assert.IsTrue(expectedId > 0);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                GetComponent<RectTransform>().anchoredPosition;
            if (eventData.pointerDrag.GetComponent<DragDrop>() != null)
            {
                _placedId = eventData.pointerDrag.GetComponent<DragDrop>().id;
            }
        }
    }
    public bool IsCorrect()
    {
        return expectedId == _placedId;
    }
}
