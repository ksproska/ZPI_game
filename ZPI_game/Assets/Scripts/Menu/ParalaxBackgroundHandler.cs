using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParalaxBackgroundHandler : MonoBehaviour
{
    [NonSerialized] public List<BackgroundScroller> layers;
    private float current = 0;

    // scrollSpeed * 10 - 200

    private void Start()
    {
        layers = new List<BackgroundScroller>(GetComponentsInChildren<BackgroundScroller>());
    }

    private void Update()
    {
        
    }

    public void SpeedUp()
    {
        layers.ForEach(l => l.SpeedUpForTime(l.scrollSpeed * 10 - 200, 0.25f));
    }
}
