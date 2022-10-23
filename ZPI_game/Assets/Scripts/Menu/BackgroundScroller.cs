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

    private (float, float) GetObjectSize()
    {
        var p1 = gameObject.transform.TransformPoint(0, 0, 0);
        var p2 = gameObject.transform.TransformPoint(1, 1, 0);
        var w = p2.x - p1.x;
        var h = p2.y - p1.y;
        return (h, w);
    }

    public void SpeedUpForTime(float speed, float time)
    {
        StartCoroutine(SpeedUp(speed, time));
    }

    private void BackToNormalSpeed()
    {
        scrollSpeed = startupSpeed;
    }

    private IEnumerator SpeedUp(float speed, float time)
    {
        scrollSpeed = speed;
        yield return new WaitForSeconds(time);
        BackToNormalSpeed();
    }
}
