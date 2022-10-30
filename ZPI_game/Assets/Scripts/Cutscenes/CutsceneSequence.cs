using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSequence : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources;
    [NonSerialized] private List<CutsceneElement> cutsceneElements;

    private void Start()
    {
        cutsceneElements = new List<CutsceneElement>(GetComponentsInChildren<CutsceneElement>());
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        foreach (var elem in cutsceneElements)
        {
            yield return StartCoroutine(elem.Play());
        }
    }
}
