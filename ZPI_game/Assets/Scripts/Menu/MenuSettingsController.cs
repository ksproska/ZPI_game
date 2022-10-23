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


    private void Start()
    {
        musicVolumeSlider.value = CurrentGameState.MusicVolume;
        musicVolumeText.text = $"{(int)(musicVolumeSlider.value * 100)}%";
        effectsVolumeSlider.value = CurrentGameState.EffectsVolume;
        effectsVolumeText.text = $"{(int)(effectsVolumeSlider.value * 100)}%";
        musicToggle.isOn = CurrentGameState.IsMusicOn;
        effectsToggle.isOn = CurrentGameState.AreEffectsOn;
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
    }

    public void OnEffectsVolumeChange(float volume)
    {
        effectsVolumeText.text = $"{(int)(effectsVolumeSlider.value * 100)}%";
    }
}
