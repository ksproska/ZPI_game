using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    private VideoPlayer player;
    private Slider _slider;
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        _slider = FindObjectOfType<Slider>();
        _slider.maxValue = (float) player.clip.length;
    }

    private void Update()
    {
        _slider.value = (float) player.time;
    }

    public void SetTime(float newTime)
    {
        player.time = newTime;
    }

    public void StartStop()
    {
        if (player.isPlaying == false)
        {
            player.Play();
        }
        else {
            player.Pause();
        }
    }
}
