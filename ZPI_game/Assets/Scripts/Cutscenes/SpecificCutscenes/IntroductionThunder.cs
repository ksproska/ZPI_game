using System;
using System.Collections;
using System.Collections.Generic;
using Cutscenes;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionThunder : MonoBehaviour, ICutscenePlayable
{
    [SerializeField] private Text text;
    [SerializeField] private string linesResourceName;
    [SerializeField] private Image fader;
    private List<string> textLines;
    private int currentIndex;

    private void Start()
    {
        var allText = Resources.Load<TextAsset>($"Cutscenes/{linesResourceName}").text;
        textLines = CutsceneFileReader.GetTextSequence(allText);
        text.text = "";
        text.color = Color.white;
    }
    
    public IEnumerator NextLine(float fadeTime)
    {
        text.CrossFadeAlpha(0, fadeTime, true);
        currentIndex += 1;
        yield return new WaitForSeconds(fadeTime);
        text.text = textLines[currentIndex];
        text.CrossFadeAlpha(1, fadeTime, true);
    }

    public IEnumerator Play()
    {
        fader.CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(2);
        text.CrossFadeAlpha(1, 1, true);
        text.canvasRenderer.SetAlpha(0);
        text.fontStyle = FontStyle.Italic;
        currentIndex = -1;
        yield return NextLine(2);
        yield return new WaitForSeconds(4);
        text.CrossFadeAlpha(0, 2, true);
    }
}
