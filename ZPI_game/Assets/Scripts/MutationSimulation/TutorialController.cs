using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TutorialController : MonoBehaviour
{

    public DropHandler dropHandler;
    [SerializeField] private GameObject staticPrefab;

    public GameObject levelButton;
    public GameObject tutorialButton;
    public GameObject checkButton;

    public CreateIntsToDrop gridCreator;

    public GameObject tutorialCanvas;
    public GameObject levelCanvas;

    public GameObject nextButton;
    public GameObject previousButton;

    private int currentStep;
    private Stack<GameObject> tutorialStack;
    // Start is called before the first frame update
    void Start()
    {
        currentStep = 0;
        tutorialStack = new();
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
        levelCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
        tutorialButton.SetActive(true);
        checkButton.SetActive(true);
        levelButton.SetActive(false);
        nextButton.SetActive(false);
        previousButton.SetActive(false);

    }

    public void NextStep()
    {
        if(currentStep < gridCreator.steps.Count)
        {
            var slots = gridCreator.allTutorialSlots;
            var drops = gridCreator.allTutorialDrops;
            var (slot, value) = gridCreator.steps[currentStep];
            var drop = drops.Where(item => item.GetComponent<TextMeshProUGUI>().text == $"{value}").First();
            //GameObject staticAdded = Instantiate(staticPrefab, slots[slot].transform.position, Quaternion.identity,
            //        tutorialCanvas.transform);
            Debug.Log(drop.transform.position);
            Debug.Log(slots[slot].transform.position);

            drop.transform.position = slots[slot].transform.position;

            Debug.Log(drop.transform.position);
            Debug.Log(slots[slot].transform.position);

            tutorialStack.Push(drop);
            currentStep += 1;

        }
    }

    public void PreviousStep()
    {
        if (currentStep > 0)
        {
            var drop = tutorialStack.Pop();
            var statics = gridCreator.allTutorialStatics;
            var singleStatic = statics.Where(item => item.GetComponent<TextMeshProUGUI>().text == $"{drop.GetComponent<TextMeshProUGUI>().text}").First();
            drop.transform.position = singleStatic.transform.position;
            currentStep -= 1;
        }
    }


}
