using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    // [SerializeField] public string contents;
    [SerializeField] public TextMeshProUGUI TextMeshPro;
    public bool isAtTheRightPosition = false;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    public CanvasGroup canvasGroup;
    private Vector3 _startPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void Start()
    {
        _startPos = GetComponent<Transform>().position;
    }

    public void SetContent(string content)
    {
        TextMeshPro.text = content;
    }

    public void OnPointerDown(PointerEventData eventData) {}

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        isAtTheRightPosition = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (!isAtTheRightPosition)
        {
            GetComponent<Transform>().position = _startPos;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        if (eventData.pointerDrag != null)
        {
            if (GetComponent<RectTransform>().position != _startPos)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().position =
                    GetComponent<RectTransform>().position;
                eventData.pointerDrag.GetComponent<DragDrop>().isAtTheRightPosition = true;
                DropSlot dropslot = FindObjectsOfType<DropSlot>().Where(ds => ds.transform.position == GetComponent<RectTransform>().position).First();
                dropslot.SetContent(eventData.pointerDrag.GetComponent<DragDrop>().TextMeshPro.text);
            }
        }

        GetComponent<Transform>().position = _startPos;
        canvasGroup.blocksRaycasts = true;
    }
}
