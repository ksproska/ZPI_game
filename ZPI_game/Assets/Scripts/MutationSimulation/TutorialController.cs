using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Assets.GA.Utils;
using GA.mutations;

public class TutorialController : MonoBehaviour
{
    System.Random rnd = new System.Random();


    public DropHandler dropHandler;

    public GameObject levelButton;
    public GameObject tutorialButton;
    public GameObject checkButton;
    public TextMeshProUGUI StartingIndexTextContainer;
    public TextMeshProUGUI EndingIndexTextContainer;



    public GameObject tutorialContainer;
    public GameObject levelContainer;

    public GameObject slotListPrefab;
    public GameObject dropsListPrefab;
    public GameObject staticsListPrefab;

    public GameObject tutorialSlotsList;
    public GameObject tutorialDropsList;
    public GameObject tutorialStaticsList;


    public GameObject nextButton;
    public GameObject previousButton;

    private int currentStep;
    private Stack<GameObject> tutorialStack;

    [SerializeField] private Material lineMaterial;

    private GameObject slider;
    private GameObject target;


    [NonSerialized] public int beginIndex;
    [NonSerialized] public int endIndex;
    [NonSerialized] public List<int> beginGenome;
    [NonSerialized] public List<int> endGenome;
    [NonSerialized] public List<(int, int)> steps;
    // Start is called before the first frame update
    void Start()
    {
        currentStep = 0;
        tutorialStack = new();
        CalculateNextCrossing();


        slotListPrefab.GetComponent<GenomSlotsCreator>().FillGenome(endGenome);
        


    }

    void CalculateNextCrossing()
    {
        beginIndex = rnd.Next(0, 9);
        do
        {
            endIndex = rnd.Next(0, 9);
        } while (endIndex - beginIndex == 0 || endIndex - beginIndex == -1);

        beginGenome = staticsListPrefab.GetComponent<GenomCreator>().genomeList;
        RecordedList<int> recordedMutation = new(beginGenome);
        endGenome = MutatorPartialReverser<int>.ReversePartOrder(beginGenome, beginIndex, endIndex, ref recordedMutation);
        steps = recordedMutation.GetFullHistory().Distinct().ToList();

        StartingIndexTextContainer.text = $"Starting index: {beginIndex}";
        EndingIndexTextContainer.text = $"Ending index: {endIndex}";
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
        DestroylevelGrid();

        levelContainer.SetActive(false);
        tutorialContainer.SetActive(true);
        tutorialDropsList.GetComponent<GenomCreator>().CreateNewGenom();
        tutorialStaticsList.GetComponent<GenomCreator>().CreateNewGenom();
        tutorialSlotsList.GetComponent<GenomSlotsCreator>().CreateSlots(endGenome);
        tutorialButton.SetActive(false);
        checkButton.SetActive(false);
        levelButton.SetActive(true);
        nextButton.SetActive(true);
        previousButton.SetActive(true);
    }

    public void LeaveTutorial()
    {
        DestroyTutorialGrid();

        staticsListPrefab.GetComponent<GenomCreator>().InitializeGenome();
        staticsListPrefab.GetComponent<GenomCreator>().CreateNewGenom();
        dropsListPrefab.GetComponent<GenomCreator>().CreateNewGenom();
        CalculateNextCrossing();
        slotListPrefab.GetComponent<GenomSlotsCreator>().CreateSlots(endGenome);

        levelContainer.SetActive(true);
        tutorialContainer.SetActive(false);
        tutorialButton.SetActive(true);
        checkButton.SetActive(true);
        levelButton.SetActive(false);
        nextButton.SetActive(false);
        previousButton.SetActive(false);
        currentStep = 0;
        tutorialStack = new();

    }

    private void DestroyTutorialGrid()
    {
        foreach (Transform child in tutorialSlotsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in tutorialDropsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in tutorialStaticsList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void DestroylevelGrid()
    {
        foreach (Transform child in dropsListPrefab.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in slotListPrefab.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in staticsListPrefab.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void NextStep()
    {
        if(currentStep < steps.Count)
        {
            previousButton.GetComponent<Button>().enabled = false;
            nextButton.GetComponent<Button>().enabled = false;
            var slots = tutorialSlotsList.GetComponent<GenomSlotsCreator>().geneList;
            var drops = tutorialDropsList.GetComponent<GenomCreator>().geneList;
            var (slot, value) = steps[currentStep];
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
            var statics = tutorialStaticsList.GetComponent<GenomCreator>().geneList;
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
