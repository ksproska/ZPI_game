using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI credits;
    [SerializeField] private float speed;
    private Vector3 startPosition;
    private string creditsText = "EMPTY";

    private void Awake()
    {
        startPosition = transform.position;
        //creditsText = Resources.Load<TextAsset>("Resources/Credits/credits").text;
        Debug.Log(creditsText);
    }

    private void Start()
    {
        credits.text = creditsText;
    }

    private void OnEnable()
    {
        transform.position = startPosition;
    }

    private void Update()
    {
        float distance = speed * Time.deltaTime;
        transform.position += new Vector3(0, distance, 0);
    }
}
