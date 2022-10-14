using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Anaglify : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text text;
    [SerializeField] float blureFactor;
    [SerializeField] Color firstColor;
    [SerializeField] Color secondColor;

    [NonSerialized] Text firstColorText;
    [NonSerialized] GameObject firstColorObject;
    [NonSerialized] Text econdColorText;
    [NonSerialized] GameObject secondColorObject;

    private void Start()
    {
        SetupAnaglifyEffect(ref firstColorObject, ref firstColorText, "RedText", firstColor);
        SetupAnaglifyEffect(ref secondColorObject, ref econdColorText, "BlueText", secondColor);
    }

    void SetupAnaglifyEffect(ref GameObject colorObject, ref Text colorText, string objectName, Color color)
    {
        colorObject = new GameObject();
        colorObject.name = objectName;
        colorObject.layer = 5;


        colorText = colorObject.AddComponent<Text>();
        colorObject.transform.SetParent(text.transform.parent);

        colorText.rectTransform.offsetMax = text.rectTransform.offsetMax;
        colorText.rectTransform.offsetMin = text.rectTransform.offsetMin;
        colorObject.transform.localPosition = new Vector3(0, 0, 0);
        colorObject.transform.localScale = gameObject.transform.localScale;
        colorObject.GetComponent<RectTransform>().SetAsFirstSibling();
        colorObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        colorObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        colorObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

        colorText.font = text.font;
        colorText.alignment = text.alignment;
        colorText.text = text.text;
        colorText.color = color;
        colorText.fontSize = text.fontSize;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        firstColorObject.transform.localPosition = new Vector3(blureFactor, 0, 0);
        secondColorObject.transform.localPosition = new Vector3(-blureFactor, blureFactor / 2f, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        firstColorObject.transform.localPosition = new Vector3(0, 0, 0);
        secondColorObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
