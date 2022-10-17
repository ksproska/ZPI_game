using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public Slider volumeSlider;
    [SerializeField] public Toggle musicPlayingToggle;

    private void Start()
    {
        volumeSlider.value = audioSource.volume;
        musicPlayingToggle.isOn = audioSource.isPlaying;
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void IsPlaying(bool isPlaying)
    {
        if(isPlaying)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
