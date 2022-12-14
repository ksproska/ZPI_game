using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class EducationDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IPointerUpHandler
{
    // [SerializeField] public string contents;
    [SerializeField] public TextMeshProUGUI TextMeshPro;
    private Canvas _canvas;
    public CanvasGroup canvasGroup;
    public GameObject prefab;
    private GameObject drop;

    private bool _isMoved = false;


    public void Update()
    {
        foreach (DragDrop child in FindObjectsOfType<DragDrop>().Where(drop => drop.transform.position == transform.position).ToList())
        {
            if (child != null)
            {
                if (child.isAtTheRightPosition && child.transform.position == transform.position)
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
            transform.parent);
        drop.GetComponent<DragDrop>().isAtTheRightPosition = false;
        drop.GetComponent<DragDrop>().SetContent(TextMeshPro.text);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _isMoved = true;
        drop.GetComponent<DragDrop>().canvasGroup.blocksRaycasts = false;
        eventData.pointerDrag = drop;
    }

    public void OnDrag(PointerEventData eventData)
    {
        drop.GetComponent<RectTransform>().anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isMoved)
        {
            Destroy(drop);
        }
        _isMoved = false;
    }
}
