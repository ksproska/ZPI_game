using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public string expectedContents;
    public string _placedContent;
    [SerializeField] public int expectedParent;
    public int _placedFromParent;


    public void OnStart()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
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
                _placedFromParent = eventData.pointerDrag.GetComponent<DragDrop>().parent;
}
            eventData.pointerDrag.GetComponent<DragDrop>().isAtTheRightPosition = true;
        }
        
    }
    public bool IsCorrect()
    {
        return expectedContents == _placedContent;
    }

    public bool IsFromCorrectParent()
    {
        return expectedParent == _placedFromParent;
    }

    public void SetContent(string content)
    {
        _placedContent = content;
    }

    public void SetParent(int parent)
    {
        _placedFromParent = parent;
    }

}
