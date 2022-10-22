using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParalaxBackgroundHandler : MonoBehaviour
{
    [NonSerialized] public List<BackgroundScroller> layers;

    private void Start()
    {
        layers = new List<BackgroundScroller>(GetComponentsInChildren<BackgroundScroller>());
        Debug.Log(layers.Count);
    }
}
