using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class EducationDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler
{
    // [SerializeField] public string contents;
    [SerializeField] public TextMeshProUGUI TextMeshPro;
    private Canvas _canvas;
    public CanvasGroup canvasGroup;
    public GameObject prefab;
    private GameObject drop;


    public void Update()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<DragDrop>() != null)
            {
                if (child.GetComponent<DragDrop>().isAtTheRightPosition && child.position == transform.position)
                {
                    Destroy(child.gameObject);
                }

            }
        }
    }

    public void Awake()
    {
        _canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetContent(string content)
    {
        TextMeshPro.text = content;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        drop = Instantiate(prefab, transform.position, Quaternion.identity,
            transform);
        drop.GetComponent<DragDrop>().isAtTheRightPosition = false;
        drop.GetComponent<DragDrop>().SetContent(TextMeshPro.text);
        eventData.pointerPress = drop;
        eventData.selectedObject = drop;
        eventData.pointerClick = drop;
        eventData.rawPointerPress = drop;
        eventData.pointerEnter = drop;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        drop.GetComponent<DragDrop>().canvasGroup.blocksRaycasts = false;
        eventData.pointerDrag = drop;
    }

    public void OnDrag(PointerEventData eventData)
    {
        drop.GetComponent<RectTransform>().anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

}
