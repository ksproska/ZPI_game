using DeveloperUtils;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerUpHandler, IPointerDownHandler
{
    // [SerializeField] public string contents;
    [SerializeField] public TextMeshProUGUI TextMeshPro;
    public bool isAtTheRightPosition = true;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    public CanvasGroup canvasGroup;
    private Vector3 _startPos;
    [NonSerialized] public int parent;


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
    public void SetParent(int parent)
    {
        this.parent = parent;
    }

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
        isAtTheRightPosition = true;
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
                DropSlot dropslot = FindObjectsOfType<DropSlot>().Where(ds => ds.GetComponent<RectTransform>().position == GetComponent<RectTransform>().position).First();


                dropslot.SetContent(eventData.pointerDrag.GetComponent<DragDrop>().TextMeshPro.text);
                dropslot.SetParent(eventData.pointerDrag.GetComponent<DragDrop>().parent);
            }
        }

        GetComponent<Transform>().position = _startPos;
        canvasGroup.blocksRaycasts = true;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Test");
        if(transform.position == _startPos)
        {
            Destroy(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Test");
    }
}
