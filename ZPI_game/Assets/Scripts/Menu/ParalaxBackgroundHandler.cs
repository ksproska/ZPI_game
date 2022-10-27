using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParalaxBackgroundHandler : MonoBehaviour
{
    [SerializeField] private List<BackgroundScroller> layers;

    private void Start()
    {
        //layers = new List<BackgroundScroller>(FindObjectsOfType<BackgroundScroller>());
    }

    public void SpeedUp()
    {
        SpeedUpForTime(0.6f);
    }

    public void SpeedUpForTime(float time)
    {
        layers.ForEach(l => l.SpeedUpForTime(l.scrollSpeed * 10 - 200, time));
    }
}
