using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class City : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private int widthScaler = 1920;
    private int heightScaler = 1080;

    [SerializeField] private Material lineMaterial;

    private LineRenderer _lineRenderer;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    [NonSerialized] public int cityNumber;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _lineRenderer = GetComponent<LineRenderer>();
    }
    
    public (float, float) GetPosition()
    {
        return (gameObject.transform.position.x, gameObject.transform.position.y);
    }

    public void DrawLine(City other)
    {
        Vector3[] pathPoints = { this.transform.position, other.transform.position };
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
        _canvasGroup.blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    }
}
