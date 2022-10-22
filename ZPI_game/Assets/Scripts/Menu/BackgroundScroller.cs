using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]
    [Range(-10f, 10f)]
    public float scrollSpeed = -0.5f;

    private float offset;
    private float startupSpeed;
    [NonSerialized] private Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        startupSpeed = scrollSpeed;
    }

    void Update()
    {
        offset += Time.deltaTime * scrollSpeed / 100f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
