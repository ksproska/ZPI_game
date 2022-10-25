using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{

    public DropHandler dropHandler;

    public GameObject levelButton;
    public GameObject tutorialButton;
    public GameObject checkButton;

    public CreateIntsToDrop gridCreator;

    public GameObject tutorialCanvas;
    public GameObject levelCanvas;

    public GameObject nextButton;
    public GameObject previousButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TutorialButtonActivation()
    {
        if (!dropHandler.AreAllCorrect())
        {
            tutorialButton.SetActive(true);
        }
    }



    public void CreateTutorial()
    {

        levelCanvas.SetActive(false);
        tutorialCanvas.SetActive(true);
        gridCreator.CreateTutorial(tutorialCanvas);
        tutorialButton.SetActive(false);
        checkButton.SetActive(false);
        levelButton.SetActive(true);
        nextButton.SetActive(true);
        previousButton.SetActive(true);
    }

    public void LeaveTutorial()
    {
        gridCreator.DestroyTutorial();
        //GameObject.Find("LevelCanvas").SetActive(true);
        //GameObject.Find("TutorialCanvas").SetActive(false);
        levelCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
        tutorialButton.SetActive(true);
        checkButton.SetActive(true);
        levelButton.SetActive(false);
        nextButton.SetActive(false);
        previousButton.SetActive(false);

    }
}
