using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Text text;
    [SerializeField] public Image background;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.white;
        background.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.black;
        background.color = Color.white;
    }
}
