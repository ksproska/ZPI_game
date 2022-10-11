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
    [NonSerialized] Text redText;
    [NonSerialized] GameObject redObject;
    [NonSerialized] Text blueText;
    [NonSerialized] GameObject blueObject;
    [NonSerialized] Vector3 viewerPoint;

    private void Start()
    {
        //SetupAnaglifyEffect(ref redObject, ref redText, "RedText", new Color(1, 0, 0));
        //viewerPoint = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        redObject = new GameObject();
        redObject.name = "RedText";
        redObject.layer = 5;


        redText = redObject.AddComponent<Text>();
        redObject.transform.SetParent(text.transform.parent);

        redText.rectTransform.offsetMax = text.rectTransform.offsetMax;
        redText.rectTransform.offsetMin = text.rectTransform.offsetMin;
        redObject.transform.localPosition = new Vector3(0, 0, 0);
        redObject.transform.localScale = gameObject.transform.localScale;
        redObject.GetComponent<RectTransform>().SetAsFirstSibling();
        redObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        redObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        redObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

        redText.font = text.font;
        redText.alignment = text.alignment;
        redText.text = text.text;
        redText.color = new Color(1, 0, 0);
        redText.fontSize = text.fontSize;

        //blueObject = new GameObject();
        //blueObject.layer = 5;
        //blueText = blueObject.AddComponent<Text>();
        //blueText.font = text.font;
        //blueText.transform.SetParent(text.transform);
        //blueText.transform.localScale = text.transform.localScale;
        //blueText.transform.localPosition = new Vector3(0, 0, 0);
        //blueText.text = text.text;
        //blueText.color = new Color(0, 1, 1);
    }

    void SetupAnaglifyEffect(ref GameObject colorObject, ref Text colorText, string objectName, Color color)
    {
        colorObject = new GameObject();
        colorObject.name = objectName;
        colorObject.layer = 5;


        colorText = redObject.AddComponent<Text>();
        colorObject.transform.SetParent(text.transform.parent);

        colorText.rectTransform.offsetMax = text.rectTransform.offsetMax;
        colorObject.transform.localPosition = new Vector3(0, 0, 0);
        colorObject.transform.localScale = gameObject.transform.localScale;
        colorObject.GetComponent<RectTransform>().SetAsFirstSibling();

        colorText.font = text.font;
        colorText.alignment = text.alignment;
        colorText.text = text.text;
        colorText.color = color;
        colorText.fontSize = text.fontSize;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //redText.rectTransform.position = new Vector3(viewerPoint.x + blureFactor, viewerPoint.y, viewerPoint.z);
        redObject.transform.localPosition = new Vector3(blureFactor, 0, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //redText.rectTransform.position = viewerPoint;
        redObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
