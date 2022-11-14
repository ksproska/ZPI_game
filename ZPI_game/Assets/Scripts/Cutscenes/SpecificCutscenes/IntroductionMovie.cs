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

    private void Awake()
    {
        CurrentGameState.Instance.CurrentLevelName = SceneManager.GetActiveScene().name;
    }

    public IEnumerator Play()
    {
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
    }
}
