using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuContainer : MonoBehaviour
{
    [NonSerialized] private List<SlidingComponent> components;

    private void Start()
    {
        components = new List<SlidingComponent>(GetComponentsInChildren<SlidingComponent>());
    }
    public void SlideOutComponents()
    {
        StartCoroutine(SlideOutDelta(0.1f));
    }

    private IEnumerator SlideOutDelta(float time)
    {
        foreach(var component in components)
        {
            component.SlideOut();
            yield return new WaitForSeconds(time);
        }
    }
}
