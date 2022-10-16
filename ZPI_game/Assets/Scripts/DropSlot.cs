using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public string expectedContents;
    public string _placedContent;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("dropped");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                GetComponent<RectTransform>().anchoredPosition;
            if (eventData.pointerDrag.GetComponent<DragDrop>() != null)
            {
                eventData.pointerDrag.GetComponent<DragDrop>().isAtTheRightPosition = true;
                _placedContent = eventData.pointerDrag.GetComponent<DragDrop>().TextMeshPro.text;
                Debug.Log(_placedContent);
            }
            eventData.pointerDrag.GetComponent<DragDrop>().isAtTheRightPosition = true;
        }
    }
    public bool IsCorrect()
    {
        return expectedContents == _placedContent;
    }
}
