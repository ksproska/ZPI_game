using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuContainer : MonoBehaviour
{
    [NonSerialized] private List<SlidingComponent> components;
    //[NonSerialized] private List<IMenuActive> menuactives;


    private void Awake()
    {
        components = new List<SlidingComponent>(GetComponentsInChildren<SlidingComponent>());
        //menuactives = new List<IMenuActive>(GetComponentsInChildren<IMenuActive>());
    }
    //private void Start()
    //{
    //    components = new List<SlidingComponent>(GetComponentsInChildren<SlidingComponent>());
    //}

    //private void OnEnable()
    //{
    //    components = new List<SlidingComponent>(GetComponentsInChildren<SlidingComponent>());
    //}

    public void BeforeSlideSettings()
    {
        gameObject.SetActive(true);
        foreach(var component in components)
        {
            component.Offset();
        }
        //foreach(var ma in menuactives)
        //{
        //    ma.SetEnabled(false);
        //}
    }
    public void SlideOutComponents()
    {
        StartCoroutine(SlideOutDelta(0.1f));
    }

    private IEnumerator SlideOutDelta(float time)
    {
        components.ForEach(c => c.SetMenuComponentEnabled(false));
        foreach(var component in components)
        {
            component.SlideOut();
            yield return new WaitForSeconds(time);
        }
        yield return StartCoroutine(Deactivate());
    }

    public void SlideInComponents()
    {
        BeforeSlideSettings();
        StartCoroutine(SlideInDelta(0.1f));
    }

    private IEnumerator SlideInDelta(float time)
    {
        components.ForEach(c => c.SetMenuComponentEnabled(false));
        foreach (var component in components)
        {
            component.SlideIn();
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Deactivate()
    {
        yield return null;
        gameObject.SetActive(false);
        foreach(var component in components)
        {
            component.SetDefaultPosition();
        }
    }
}
