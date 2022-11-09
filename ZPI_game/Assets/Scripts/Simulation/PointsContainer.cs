using System;
using System.Collections;
using System.Collections.Generic;
using DeveloperUtils;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointsContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsMouseOver { get; set; } = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOver = false;
    }
}
