using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public string expectedContents;
    public string _placedContent;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position =
                GetComponent<RectTransform>().position;
            if (eventData.pointerDrag.GetComponent<DragDrop>() != null)
            {
                eventData.pointerDrag.GetComponent<DragDrop>().isAtTheRightPosition = true;
                _placedContent = eventData.pointerDrag.GetComponent<DragDrop>().TextMeshPro.text;
            }
            eventData.pointerDrag.GetComponent<DragDrop>().isAtTheRightPosition = true;
        }
    }
    public bool IsCorrect()
    {
        return expectedContents == _placedContent;
    }

    
}
