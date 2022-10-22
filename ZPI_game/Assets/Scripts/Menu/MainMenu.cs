using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public ParalaxBackgroundHandler backgroundHandler;
    [SerializeField] public List<MenuContainer> containers;


    [NonSerialized] private int currentContainer = 0;
    [NonSerialized] private int screenWidth;

    public void SpeedUp()
    {
        backgroundHandler.SpeedUp();
    }

    public void SlideOut()
    {
        containers[0].SlideOutComponents();
    }

    public void QuitApp()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
