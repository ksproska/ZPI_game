using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeveloperUtils;
using UnityEngine;
using UnityEngine.UI;

public class DrawGraph : MonoBehaviour
{
    private Canvas _canvas;
    // [SerializeField] Image graphBackground;
    [SerializeField] private GameObject leftCorner, rightCorner;
    [SerializeField] private LineRenderer lineRenderer, bestLineRenderer;
    private float xMin, xMax, yMin, yMax;
    private List<float> x = new List<float>(){};
    private List<float> y = new List<float>(){};
    private float bestValue = int.MaxValue;

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        xMin = leftCorner.transform.position.x;
        xMax = rightCorner.transform.position.x;
        yMin = leftCorner.transform.position.y;
        yMax = rightCorner.transform.position.y;
    }

    public void AddPointAndUpdate(float xp, float yp)
    {
        x.Add(xp);
        y.Add(yp);
        SetGraph();
    }

    public void RemoveAllPoints()
    {
        if (y.Count > 0 && y[^1] < bestValue)
        {
            bestValue = y[^1];
        }
        x.Clear();
        y.Clear();
    }

    private void SetGraph()
    {
        lineRenderer.positionCount = Math.Max(x.Count, 2);
        var width = xMax - xMin;
        var height = yMax - yMin;
        for (int i = 0; i < x.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(xMin + width * (x[i] / x.Max()), yMin + height * (y[i] / y.Max()), -9));
        }
        
        if (bestValue < int.MaxValue)
        {
            bestLineRenderer.positionCount = 2;
            bestLineRenderer.SetPosition(0, new Vector3(xMin, yMin + height * (bestValue / y.Max()), -9));
            bestLineRenderer.SetPosition(1, new Vector3(xMax, yMin + height * (bestValue / y.Max()), -9));
        }
    }

    public void ClearBest()
    {
        bestValue = int.MaxValue;
        bestLineRenderer.positionCount = 0;
    }
}
