using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IMenuActive
{
    [SerializeField] public Text text;
    [SerializeField] public Image background;
    [NonSerialized] private bool isActive = true;

    public bool IsEnabled => isActive;

    private void OnEnable()
    {
        text.color = Color.black;
        background.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive) return;
        text.color = Color.white;
        background.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive) return;
        text.color = Color.black;
        background.color = Color.white;
    }

    public void SetEnabled(bool isActive)
    {
        this.isActive = isActive;
        GetComponent<Button>().interactable = isActive;
    }
}
