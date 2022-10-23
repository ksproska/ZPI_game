using System;
using System.Collections;
using System.Collections.Generic;
using CurrentState;
using UnityEngine;
using UnityEngine.UI;
using System;

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
        musicVolumeSlider.value = CurrentGameState.MusicVolume;
        musicVolumeText.text = $"{(int)(musicVolumeSlider.value * 100)}%";
        effectsVolumeSlider.value = CurrentGameState.EffectsVolume;
        effectsVolumeText.text = $"{(int)(effectsVolumeSlider.value * 100)}%";
        musicToggle.isOn = CurrentGameState.IsMusicOn;
        effectsToggle.isOn = CurrentGameState.AreEffectsOn;
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
            source.Pause();
        }
    }

    public void SaveChanges()
    {
        CurrentGameState.MusicVolume = musicVolumeSlider.value;
        CurrentGameState.EffectsVolume = effectsVolumeSlider.value;
        CurrentGameState.IsMusicOn = musicToggle.isOn;
        CurrentGameState.AreEffectsOn = effectsToggle.isOn;
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
            source.Pause();
        }
    }

    public void OnEffectsToggleChange(bool isOn)
    {
        musicToggle.isOn = isOn;
    }
}
