using CurrentState;
using LevelUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private Vector3 expectedPosition;
    private Vector3 temporaryPosition;
    public GameObject popupMenu;
    public GameObject settingsMenu;
    public GameObject container;
    public GoToScene goToScene;
    public Text text;
    [SerializeField] private GameObject mouth;

    private bool rollInMenu = false;

    private RectTransform _rectTransform;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        settingsMenu.SetActive(false);
        popupMenu.SetActive(true);
        expectedPosition = container.GetComponent<RectTransform>().localPosition;
        container.GetComponent<RectTransform>().localPosition += Vector3.up * container.GetComponent<RectTransform>().rect.height * container.GetComponent<RectTransform>().localScale.y;
        temporaryPosition = container.GetComponent<RectTransform>().localPosition;
        if(CurrentGameState.Instance.CurrentLevelName != "WorldMap")
        {
            var lvlName = CurrentGameState.Instance.CurrentLevelName;
            if (LevelMap.IsNavigableFromMap(lvlName))
                lvlName = LevelMap.GetClearMapName(lvlName);
            text.text = lvlName;
        }
        else
        {
            text.text = "WorldMap";
        }

        mouth.SetActive(CurrentGameState.Instance.AreEffectsOn);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (!rollInMenu)
            {
                rollInMenu = true;
            }
            GetComponent<MenuSettingsController>().SaveChanges();
        }

        if (rollInMenu)
        {
            if (!compareVector3(container.GetComponent<RectTransform>().localPosition, expectedPosition))
            {
                container.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(container.GetComponent<RectTransform>().localPosition, expectedPosition, Time.deltaTime * 3000);
            }
            else
            {
                rollInMenu = false;
                (expectedPosition, temporaryPosition) = (temporaryPosition, expectedPosition);
                settingsMenu.SetActive(false);
                popupMenu.SetActive(true);
            }

        }
        

    }

    public void RollInMenu()
    {
        rollInMenu = true;
        GetComponent<MenuSettingsController>().SaveChanges();
    }

    public void GoToScene(string mapName)
    {
        goToScene.scene = mapName;
        goToScene.FadeOutScene();
    }

    public void Restart()
    {
        goToScene.scene = CurrentGameState.Instance.CurrentLevelName;
        goToScene.FadeOutScene();
    }

    public void SwapToSettings()
    {
        popupMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void SwapToMenu()
    {
        settingsMenu.SetActive(false);
        popupMenu.SetActive(true);
        GetComponent<MenuSettingsController>().SaveChanges();
    }

    private bool compareVector3(Vector3 vector3, Vector3 otherVector3)
    {
        return (Math.Abs(vector3[1] - otherVector3[1]) < 0.1f);
    }

    public void SwapMouthVisibility(bool isActive)
    {
        mouth.SetActive(isActive);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
