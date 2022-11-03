using System;
using System.Collections;
using System.Collections.Generic;
using Cutscenes;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionThunder : MonoBehaviour, ICutscenePlayable
{
    [SerializeField] private Text text;
    [SerializeField] private Image fader;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private List<string> textLines;
    private int currentIndex;

    private void Awake()
    {
        text.canvasRenderer.SetAlpha(0);
    }
    
    public IEnumerator ShowText(float fadeTime)
    {
        text.CrossFadeAlpha(1, fadeTime, true);
        yield return new WaitForSeconds(3);
        text.CrossFadeAlpha(0, fadeTime, true);
        yield return new WaitForSeconds(fadeTime);
    }

    public IEnumerator Play()
    {
        text.canvasRenderer.SetAlpha(0);
        audioSource.clip = audioClip;
        audioSource.Play();
        fader.canvasRenderer.SetAlpha(0);
        fader.CrossFadeAlpha(0.5f, 2, true);
        yield return new WaitForSeconds(1);
        text.CrossFadeAlpha(1, 1, true);
        yield return ShowText(2);
        fader.CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(1f);
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime;
            yield return null;
        }
        audioSource.Pause();
    }
}
