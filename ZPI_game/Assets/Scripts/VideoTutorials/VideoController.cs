using System;
using DeveloperUtils;
using LevelUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private Button finishWatching;
    [SerializeField] private Slider slider;

    [SerializeField] float slideDuration;

    private bool isPlaying = true;

    private void Start()
    {
        finishWatching.gameObject.SetActive(false);
        var sceneName = SceneManager.GetActiveScene().name;
        // if(LevelMap.Instance.IsLevelDone(LevelMap.GetClearMapName(sceneName), CurrentState.CurrentGameState.Instance.CurrentSlot))
        // {
        //     finishWatching.gameObject.SetActive(true);
        // }
        slider.maxValue = (float) player.clip.length;
        slider.maxValue.Debug();
    }

    private void Update()
    {
        if(player.isPlaying)
            slider.value = (float)player.time;
        if(player.time + 5 > player.clip.length)
        {
            finishWatching.gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        player.Pause();
        player.time = 0;
        player.Play();
    }
    
    public void Next()
    {
        isPlaying = player.isPlaying;
        player.Pause();
        var time = Math.Min(player.time + slideDuration, player.clip.length);
        player.time = time;
        slider.value = (float)time;
        if(isPlaying)
            player.Play();
    }

    public void Previous()
    {
        isPlaying = player.isPlaying;
        player.Pause();
        var time = Math.Max(player.time - slideDuration, 0);
        player.time = time;
        slider.value = (float)time;
        if(isPlaying)
            player.Play();
    }

    public void StartStop()
    {
        if (!player.isPlaying)
        {
            player.Play();
            isPlaying = true;
        }
        else {
            player.Pause();
            isPlaying = false;
        }
    }

    public void OnStartSliderDrag()
    {
        player.Pause();
    }

    public void OnEndSliderDrag()
    {
        player.time = slider.value;
        if(isPlaying)
            player.Play();
    }

    public void FinishWatching()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        LevelMap.Instance.CompleteALevel(sceneName, CurrentState.CurrentGameState.Instance.CurrentSlot);
    }
}
