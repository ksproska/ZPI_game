using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DeveloperUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI credits;
    [SerializeField] private float speed;
    private Vector3 startPosition;
    private float windowSize;
    private float startSize;
    private string creditsText = "EMPTY";
    private float time = 2;

    private void Awake()
    {
        windowSize = GetComponent<RectTransform>().rect.height;
        startPosition = GetComponent<RectTransform>().anchoredPosition3D;
        creditsText = Resources.Load<TextAsset>("Credits/credits").text;
        credits.text = creditsText;
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        time = 2;
        Canvas.ForceUpdateCanvases();
        startSize = credits.gameObject.GetComponent<RectTransform>().rect.height;
        GetComponent<RectTransform>().anchoredPosition3D = startPosition + Vector3.down * (startSize / 2 + windowSize/2);

    }

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {  
            startSize = credits.gameObject.GetComponent<RectTransform>().rect.height;
            float distance = speed * Time.deltaTime;
            GetComponent<RectTransform>().anchoredPosition3D += new Vector3(0, distance, 0);
            if (GetComponent<RectTransform>().anchoredPosition3D.y > startPosition.y + startSize / 2 + windowSize / 2)
            {
                GetComponent<RectTransform>().anchoredPosition3D = startPosition + Vector3.down * (startSize / 2 + windowSize / 2);
            }
        }

    }
}
