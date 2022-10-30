using System;
using System.Collections;
using System.Collections.Generic;
using Cutscenes;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneElement : MonoBehaviour
{
    private ICutscenePlayable playable;

    private void Start()
    {
        playable = GetComponent<ICutscenePlayable>();
    }

    public IEnumerator Play()
    {
        yield return StartCoroutine(playable.Play());
    }
}
