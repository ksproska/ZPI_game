using DeveloperUtils;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
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
        _startPos = transform.position;
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

        var dropSlots = FindObjectsOfType<DropSlot>().Where(ds => compareVector3(ds.transform.position));
        Debug.Log(dropSlots.Count());
        if(dropSlots.Count() != 0) {
            dropSlots.First().SetContent("");
        }
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
            transform.position = _startPos;
        }
        isAtTheRightPosition = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        if (eventData.pointerDrag != null)
        {
            if (transform.position != _startPos)
            {

                eventData.pointerDrag.transform.position =
                    transform.position;
                eventData.pointerDrag.GetComponent<DragDrop>().isAtTheRightPosition = true;
                DropSlot dropslot = FindObjectsOfType<DropSlot>().Where(ds => compareVector3(ds.transform.position)).First();


                dropslot.SetContent(eventData.pointerDrag.GetComponent<DragDrop>().TextMeshPro.text);
                dropslot.SetParent(eventData.pointerDrag.GetComponent<DragDrop>().parent);
            }
        }

        GetComponent<Transform>().position = _startPos;
        canvasGroup.blocksRaycasts = true;
    }

    private bool compareVector3(Vector3 otherVector3)
    {
        return (Math.Abs(transform.position[0] - otherVector3[0]) < 0.1f && Math.Abs(transform.position[1] - otherVector3[1]) < 0.1f);
    }


}
