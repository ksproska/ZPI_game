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
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource effectAudioSource;


    private void Start()
    {
        musicVolumeSlider.value = CurrentGameState.Instance.MusicVolume;
        musicVolumeText.text = $"{(int)(musicVolumeSlider.value * 100)}%";

        effectsVolumeSlider.value = CurrentGameState.Instance.EffectsVolume;
        effectsVolumeText.text = $"{(int)(effectsVolumeSlider.value * 100)}%";

        musicToggle.isOn = CurrentGameState.Instance.IsMusicOn;
        effectsToggle.isOn = CurrentGameState.Instance.AreEffectsOn;
        
        musicAudioSource.volume = musicVolumeSlider.value;

        if (effectAudioSource != null)
            effectAudioSource.volume = effectsVolumeSlider.value;



        if (musicToggle.isOn)
        {
            if (!musicAudioSource.isPlaying)
            {
                musicAudioSource.Play();
            }
        }
        else
        {
            musicAudioSource.Stop();
        }

        if (effectAudioSource == null) return;
        if (effectsToggle.isOn)
        {
            effectAudioSource.volume = effectsVolumeSlider.value;
        }
        else
        {
            effectAudioSource.volume = 0.0f;
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
        musicAudioSource.volume = volume;
    }

    public void OnEffectsVolumeChange(float volume)
    {
        effectsVolumeText.text = $"{(int)(effectsVolumeSlider.value * 100)}%";
        if (effectAudioSource != null)
            effectAudioSource.volume = volume;
    }

    public void OnMusicToggleChange(bool isOn)
    {
        musicToggle.isOn = isOn;
        if (isOn)
        {
            musicAudioSource.Play();
        }
        else
        {
            musicAudioSource.Stop();
        }
    }

    public void OnEffectsToggleChange(bool isOn)
    {
        effectsToggle.isOn = isOn;
        if (effectAudioSource == null) return;
        if (effectsToggle.isOn)
        {
            effectAudioSource.volume = effectsVolumeSlider.value;
        }
        else
        {
            effectAudioSource.volume = 0.0f;
        }
    }
}
