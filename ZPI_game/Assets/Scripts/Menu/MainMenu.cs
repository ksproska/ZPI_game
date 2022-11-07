using LevelUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using CurrentState;
using DeveloperUtils;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public ParalaxBackgroundHandler backgroundHandler;
    [SerializeField] public List<MenuContainer> containers;


    [NonSerialized] private int currentContainer;
    [NonSerialized] private int screenWidth;

    private void Start()
    {
        var source = GetComponent<AudioSource>();
        source.volume = CurrentGameState.Instance.MusicVolume;
        if (CurrentGameState.Instance.IsMusicOn)
        {
            source.Play();
        }
        else
        {
            source.Pause();
        }
    }

    public void SpeedUp()
    {
        backgroundHandler.SpeedUp();
    }

    public void SpeedUpfForTime(float time)
    {
        backgroundHandler.SpeedUpForTime(time);
    }

    public void SlideOut(int nextContainer)
    {
        containers[currentContainer].SlideOutComponents();
        containers[nextContainer].SlideInComponents();
        currentContainer = nextContainer;
    }

    public void QuitApp()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void DeleteSave(LoadSaveHelper.SlotNum slotNumber)
    {
        LoadSaveHelper.Instance.EraseASlot(slotNumber);
    }
}
