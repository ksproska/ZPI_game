using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Assets.GA.Utils;
using GA;

public class CrossoverTutorialController : MonoBehaviour
{
    System.Random rnd = new System.Random();


    public DropHandler dropHandler;

    public GameObject levelButton;
    public GameObject tutorialButton;
    public GameObject checkButton;
    public TextMeshProUGUI startingIndexTextContainer;
    public TextMeshProUGUI segmentLengthTextContainer;

    public GameObject tutorialContainer;
    public GameObject levelContainer;

    public GameObject parent1DropsList;
    public GameObject parent2DropsList;
    public GameObject parent1StaticsList;
    public GameObject parent2StaticsList;
    public GameObject levelSlotsList;


    public GameObject tutorialSlotsList;
    public GameObject parent1tutorialDropsList;
    public GameObject parent2tutorialDropsList;
    public GameObject parent1tutorialStaticsList;
    public GameObject parent2tutorialStaticsList;


    public GameObject nextButton;
    public GameObject previousButton;

    private int currentStep;
    private Stack<GameObject> tutorialStack;

    [SerializeField] private Material lineMaterial;

    private GameObject slider;
    private GameObject target;


    [NonSerialized] public int beginIndex;
    [NonSerialized] public int segmentLength;
    [NonSerialized] public List<int> parent1Genome;
    [NonSerialized] public List<int> parent2Genome;
    [NonSerialized] public List<int> childGenome;
    [NonSerialized] public List<(int, int, int)> steps;
    // Start is called before the first frame update
    void Start()
    {
        currentStep = 0;
        tutorialStack = new();
        CalculateNextCrossing();


        levelSlotsList.GetComponent<GenomSlotsCreator>().FillGenome(childGenome);


        childGenome = new List<int>(new int[10]);


    }

    void CalculateNextCrossing()
    {
        beginIndex = rnd.Next(0, 8);
        segmentLength = rnd.Next(1, 9-beginIndex-1);

        parent1Genome = parent1StaticsList.GetComponent<GenomCreator>().genomeList;
        parent2Genome = parent2StaticsList.GetComponent<GenomCreator>().genomeList;

        LabeledRecordedList<int, int> recordedCrossing = new(childGenome);
        childGenome = CrosserPartiallyMatched.Cross(parent1Genome, parent2Genome, beginIndex, segmentLength);
        steps = recordedCrossing.GetFullHistory().Distinct().ToList();

        startingIndexTextContainer.text = $"Starting index: {beginIndex}";
        segmentLengthTextContainer.text = $"Segment length: {segmentLength}";
    }

    // Update is called once per frame
    void Update()
    {
        if (slider != null && target != null)
        {
            if (slider.transform.position != target.transform.position)
            {
                slider.transform.position = Vector3.MoveTowards(slider.transform.position, target.transform.position, Time.deltaTime * 5);
            }
            else
            {
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
        parent1tutorialDropsList.GetComponent<GenomCreator>().CreateNewGenom();
        parent1tutorialStaticsList.GetComponent<GenomCreator>().CreateNewGenom(); 
        parent2tutorialDropsList.GetComponent<GenomCreator>().CreateNewGenom();
        parent2tutorialStaticsList.GetComponent<GenomCreator>().CreateNewGenom();
        tutorialSlotsList.GetComponent<GenomSlotsCreator>().CreateSlots(childGenome);
        tutorialButton.SetActive(false);
        checkButton.SetActive(false);
        levelButton.SetActive(true);
        nextButton.SetActive(true);
        previousButton.SetActive(true);
    }

    public void LeaveTutorial()
    {
        DestroyTutorialGrid();

        parent1StaticsList.GetComponent<GenomCreator>().InitializeGenome();
        parent1StaticsList.GetComponent<GenomCreator>().CreateNewGenom();
        parent1DropsList.GetComponent<GenomCreator>().CreateNewGenom();
        
        parent2StaticsList.GetComponent<GenomCreator>().InitializeGenome();
        parent2StaticsList.GetComponent<GenomCreator>().CreateNewGenom();
        parent2DropsList.GetComponent<GenomCreator>().CreateNewGenom();

        CalculateNextCrossing();
        levelSlotsList.GetComponent<GenomSlotsCreator>().CreateSlots(childGenome);

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
        foreach (Transform child in parent1tutorialDropsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in parent2tutorialDropsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in parent1tutorialStaticsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in parent2tutorialStaticsList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void DestroylevelGrid()
    {
        foreach (Transform child in parent1DropsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in parent2DropsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in levelSlotsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in parent1StaticsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in parent2StaticsList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void NextStep()
    {
        if (currentStep < steps.Count)
        {
            previousButton.GetComponent<Button>().enabled = false;
            nextButton.GetComponent<Button>().enabled = false;
            var slots = tutorialSlotsList.GetComponent<GenomSlotsCreator>().geneList;
            var drops = parent1tutorialDropsList.GetComponent<GenomCreator>().geneList;
            var (slot, value, parent) = steps[currentStep];
            var drop = drops.Where(item => item.GetComponent<TextMeshProUGUI>().text == $"{value}").First();
            LineRenderer lineRenderer = drop.GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
            Vector3[] pathPoints = { drop.transform.position - new Vector3(0, 0.63f), slots[slot].transform.position + new Vector3(0, 0.63f) };
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
            var statics = parent1tutorialStaticsList.GetComponent<GenomCreator>().geneList;
            var singleStatic = statics.Where(item => item.GetComponent<TextMeshProUGUI>().text == $"{drop.GetComponent<TextMeshProUGUI>().text}").First();

            LineRenderer lineRenderer = drop.GetComponent<LineRenderer>();
            Vector3[] pathPoints = { drop.transform.position + new Vector3(0, 0.63f), singleStatic.transform.position - new Vector3(0, 0.63f) };
            lineRenderer.SetPositions(pathPoints);
            lineRenderer.enabled = true;

            slider = drop;
            target = singleStatic;
            //drop.transform.position = singleStatic.transform.position;

            currentStep -= 1;
        }
    }
}
