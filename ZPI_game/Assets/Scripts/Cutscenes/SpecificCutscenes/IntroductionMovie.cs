using System;
using System.Collections;
using System.Collections.Generic;
using Cutscenes;
using UnityEngine;
using UnityEngine.Video;

public class IntroductionMovie : MonoBehaviour, ICutscenePlayable
{
    [SerializeField] private VideoPlayer videoPlayer;

    private void Awake()
    {
        
    }

    public IEnumerator Play()
    {
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
    }
}
