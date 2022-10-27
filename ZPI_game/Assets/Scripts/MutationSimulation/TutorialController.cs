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
    public TextMeshProUGUI StartingIndexTextContainer;
    public TextMeshProUGUI EndingIndexTextContainer;


    public CreateIntsToDrop gridCreator;

    public GameObject tutorialCanvas;
    public GameObject levelCanvas;

    public GameObject nextButton;
    public GameObject previousButton;

    private int currentStep;
    private Stack<GameObject> tutorialStack;

    [SerializeField] private Material lineMaterial;

    private GameObject slider;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        currentStep = 0;
        tutorialStack = new();
        StartingIndexTextContainer.text = $"Starting index: {gridCreator.beginIndex}";
        EndingIndexTextContainer.text = $"Ending index: {gridCreator.endIndex}";
        
    }

    // Update is called once per frame
    void Update()
    {
        if(slider != null && target != null)
        {
            if(slider.transform.position != target.transform.position)
            {
                slider.transform.position = Vector3.MoveTowards(slider.transform.position, target.transform.position, Time.deltaTime*5);
            }
            else{
                slider.GetComponent<LineRenderer>().enabled = false;
                slider = null;
                target = null;
                previousButton.GetComponent<Button>().enabled = true;
                nextButton.GetComponent<Button>().enabled = true;
            }
        }
        
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
        currentStep = 0;

    }

    public void NextStep()
    {
        if(currentStep < gridCreator.steps.Count)
        {
            previousButton.GetComponent<Button>().enabled = false;
            nextButton.GetComponent<Button>().enabled = false;
            var slots = gridCreator.allTutorialSlots;
            var drops = gridCreator.allTutorialDrops;
            var (slot, value) = gridCreator.steps[currentStep];
            var drop = drops.Where(item => item.GetComponent<TextMeshProUGUI>().text == $"{value}").First();
            LineRenderer lineRenderer = drop.GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
            Vector3[] pathPoints = { drop.transform.position - new Vector3(0,0.63f), slots[slot].transform.position + new Vector3(0, 0.63f) };
            lineRenderer.SetPositions(pathPoints);

            slider = drop;
            target = slots[slot];
            //drop.transform.position = slots[slot].transform.position;


            tutorialStack.Push(drop);
            currentStep += 1;

        }
    }

    public void PreviousStep()
    {
        if (currentStep > 0)
        {
            previousButton.GetComponent<Button>().enabled = false;
            nextButton.GetComponent<Button>().enabled = false;
            var drop = tutorialStack.Pop();
            var statics = gridCreator.allTutorialStatics;
            var singleStatic = statics.Where(item => item.GetComponent<TextMeshProUGUI>().text == $"{drop.GetComponent<TextMeshProUGUI>().text}").First();

            LineRenderer lineRenderer = drop.GetComponent<LineRenderer>();
            Vector3[] pathPoints = {drop.transform.position + new Vector3(0, 0.63f), singleStatic.transform.position - new Vector3(0, 0.63f) };
            lineRenderer.SetPositions(pathPoints);
            lineRenderer.enabled = true;

            slider = drop;
            target = singleStatic;
            //drop.transform.position = singleStatic.transform.position;

            currentStep -= 1;
        }
    }


}
