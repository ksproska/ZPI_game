using System;
using System.Collections;
using System.Collections.Generic;
using DeveloperUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class City : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // private int widthScaler = 1920;
    // private int heightScaler = 1080;

    [SerializeField] private Material lineMaterial;

    private LineRenderer _lineRenderer;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    [NonSerialized] public  PointsContainer container;
    [NonSerialized] public int cityNumber;

    private Vector2 oryginalPosition;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _lineRenderer = GetComponent<LineRenderer>();
        container = GetComponentInParent<PointsContainer>();
        oryginalPosition = _rectTransform.anchoredPosition;
    }
    
    public (float, float) GetPosition()
    {
        //return (_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y);
        return (gameObject.transform.position.x, gameObject.transform.position.y);
    }

    public void DrawLine(City other)
    {
        Vector3[] pathPoints = { transform.position, other.transform.position };
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPositions(pathPoints);
        _lineRenderer.material = lineMaterial;
    }

    public float Distance(City other)
    {
        var (otherX, otherY) = other.GetPosition();
        var (thisX, thisY) = GetPosition();
        return Mathf.Sqrt((otherX - thisX) * (otherX - thisX) + (otherY - thisY) * (otherY - thisY));
    }
    
    public void OnPointerDown(PointerEventData eventData) {}

    public void OnBeginDrag(PointerEventData eventData)
    {
        oryginalPosition = _rectTransform.anchoredPosition;
        _canvasGroup.blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!container.IsMouseOver)
        {
            _rectTransform.anchoredPosition = oryginalPosition;
        }
        _canvasGroup.blocksRaycasts = true;
    }
}
