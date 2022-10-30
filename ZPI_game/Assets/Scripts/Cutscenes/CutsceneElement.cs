using System;
using System.Collections;
using System.Collections.Generic;
using Cutscenes;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneElement : MonoBehaviour
{
    private ICutscenePlayable playable;

    private void Awake()
    {
        playable = GetComponent<ICutscenePlayable>();
    }

    private void Start()
    {
        //playable = GetComponent<ICutscenePlayable>();
    }

    public IEnumerator Play()
    {
        //playable ??= GetComponent<ICutscenePlayable>();
        if(playable != null)
            yield return StartCoroutine(playable.Play());
    }
}
