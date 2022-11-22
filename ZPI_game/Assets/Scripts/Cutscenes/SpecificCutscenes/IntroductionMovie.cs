using System;
using System.Collections;
using System.Collections.Generic;
using CurrentState;
using Cutscenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroductionMovie : MonoBehaviour, ICutscenePlayable
{
    [SerializeField] private VideoPlayer videoPlayer;

    private float lastVolume;

    private void Awake()
    {
        CurrentGameState.Instance.CurrentLevelName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        var effectsVolume = CurrentGameState.Instance.EffectsVolume;
        var areEffectsOn = CurrentGameState.Instance.AreEffectsOn;
        if(areEffectsOn)
        {
            effectsVolume = 0;
        }
        lastVolume = effectsVolume;
        videoPlayer.SetDirectAudioVolume(0, effectsVolume);
    }

    private void Update()
    {
        if(CurrentGameState.Instance.AreEffectsOn)
        {
            var effectsVolume = CurrentGameState.Instance.EffectsVolume;
            if (effectsVolume != lastVolume)
            {
                videoPlayer.SetDirectAudioVolume(0, effectsVolume);
                lastVolume = effectsVolume;
            }
        }
    }

    public IEnumerator Play()
    {
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
    }
}
