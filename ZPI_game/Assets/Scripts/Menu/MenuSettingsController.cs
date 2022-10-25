using System;
using System.Collections;
using System.Collections.Generic;
using CurrentState;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettingsController : MonoBehaviour
{

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle effectsToggle;
    [SerializeField] private Text musicVolumeText;
    [SerializeField] private Text effectsVolumeText;
    [SerializeField] private AudioSource source;

    private void Start()
    {
        musicVolumeSlider.value = CurrentGameState.Instance.MusicVolume;
        musicVolumeText.text = $"{(int)(musicVolumeSlider.value * 100)}%";
        effectsVolumeSlider.value = CurrentGameState.Instance.EffectsVolume;
        effectsVolumeText.text = $"{(int)(effectsVolumeSlider.value * 100)}%";
        musicToggle.isOn = CurrentGameState.Instance.IsMusicOn;
        effectsToggle.isOn = CurrentGameState.Instance.AreEffectsOn;
        source.volume = musicVolumeSlider.value;
        if (musicToggle.isOn)
        {
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
        else
        {
            source.Stop();
        }
    }

    public void SaveChanges()
    {
        CurrentGameState.Instance.MusicVolume = musicVolumeSlider.value;
        CurrentGameState.Instance.EffectsVolume = effectsVolumeSlider.value;
        CurrentGameState.Instance.IsMusicOn = musicToggle.isOn;
        CurrentGameState.Instance.AreEffectsOn = effectsToggle.isOn;
    }

    public void OnMusicVolumeChange(float volume)
    {
        musicVolumeText.text = $"{(int)(musicVolumeSlider.value * 100)}%";
        source.volume = volume;
    }

    public void OnEffectsVolumeChange(float volume)
    {
        effectsVolumeText.text = $"{(int)(effectsVolumeSlider.value * 100)}%";
    }

    public void OnMusicToggleChange(bool isOn)
    {
        musicToggle.isOn = isOn;
        if (isOn)
        {
            source.Play();
        }
        else
        {
            source.Stop();
        }
    }

    public void OnEffectsToggleChange(bool isOn)
    {
        effectsToggle.isOn = isOn;
    }
}
