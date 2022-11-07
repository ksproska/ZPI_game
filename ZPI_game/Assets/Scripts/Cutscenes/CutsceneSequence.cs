using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsceneSequence : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources;
    [NonSerialized] private List<CutsceneElement> cutsceneElements;
    [SerializeField] private GoToScene goToScene;

    private void Start()
    {
        cutsceneElements = new List<CutsceneElement>(GetComponentsInChildren<CutsceneElement>());
        cutsceneElements.ForEach(elem => elem.gameObject.SetActive(false));
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        foreach (var elem in cutsceneElements)
        {
            elem.gameObject.SetActive(true);
            yield return StartCoroutine(elem.Play());
            if (elem != cutsceneElements.Last())
                elem.gameObject.SetActive(false);
        }
        goToScene.FadeOutScene();
    }
}
