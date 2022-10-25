using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParalaxBackgroundHandler : MonoBehaviour
{
    [NonSerialized] private List<BackgroundScroller> layers;

    private void Start()
    {
        layers = new List<BackgroundScroller>(GetComponentsInChildren<BackgroundScroller>());
        var cameraPosition = Camera.main.transform.position;
        transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0);
        Debug.Log(Application.persistentDataPath);
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
