using CurrentState;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private Vector3 expectedPosition;
    private Vector3 temporaryPosition;
    public GameObject popupMenu;
    public GameObject settingsMenu;
    public GoToScene goToScene;

    private bool rollInMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        expectedPosition = popupMenu.transform.position;
        popupMenu.transform.position += Vector3.up * popupMenu.GetComponent<RectTransform>().rect.height;
        settingsMenu.transform.position += Vector3.up * settingsMenu.GetComponent<RectTransform>().rect.height;
        temporaryPosition = popupMenu.transform.position;
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
        }

        if (rollInMenu)
        {
            if (!compareVector3(popupMenu.transform.position, expectedPosition))
            {
                Debug.Log(popupMenu.transform.position);
                Debug.Log(expectedPosition);

                popupMenu.transform.position = Vector3.MoveTowards(popupMenu.transform.position, expectedPosition, Time.deltaTime * 3000);
                settingsMenu.transform.position = Vector3.MoveTowards(settingsMenu.transform.position, expectedPosition, Time.deltaTime * 3000);
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
    }

    private bool compareVector3(Vector3 vector3, Vector3 otherVector3)
    {
        return (Math.Abs(vector3[1] - otherVector3[1]) < 0.1f);
    }

}
