using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlidingComponent : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [NonSerialized] private Vector2 targetPosition;

    private float slideSpeed = 20;
    private float screenOffset = 1_000;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        targetPosition = rectTransform.anchoredPosition;
    }
    public void SlideOut()
    {
        StartCoroutine(SlideOutLoop(slideSpeed));
    }

    public void Offset()
    {
        rectTransform.anchoredPosition = targetPosition + new Vector2(screenOffset, 0);
        SetMenuComponentEnabled(false);
    }

    public void SetDefaultPosition()
    {
        rectTransform.anchoredPosition = targetPosition;
    }

    public void SlideIn()
    {
        StartCoroutine(SlideInLoop(slideSpeed));
    }

    private IEnumerator SlideOutLoop(float ammount)
    {
        SetMenuComponentEnabled(false);
        var x = rectTransform.anchoredPosition.x;
        while (x > -screenOffset)
        {
            rectTransform.anchoredPosition -= new Vector2(ammount, 0);
            x -= ammount;
            yield return null;
        }
    }

    private IEnumerator SlideInLoop(float ammount)
    {
        SetMenuComponentEnabled(false);
        var x = rectTransform.anchoredPosition.x;
        while (x > targetPosition.x)
        {
            rectTransform.anchoredPosition -= new Vector2(ammount, 0);
            x -= ammount;
            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition;
        SetMenuComponentEnabled(true);
    }

    private void SetMenuComponentEnabled(bool isEnabled)
    {
        IMenuActive elem = GetComponent<IMenuActive>();
        if (elem != null)
        {
            elem.SetEnabled(isEnabled);
        }
    }
}
