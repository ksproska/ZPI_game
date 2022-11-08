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


    public TutorialHandler tutorialHandler;

    public TextMeshProUGUI startingIndexTextContainer;
    public TextMeshProUGUI segmentLengthTextContainer;


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
        childGenome = new List<int>(new int[10]);
        CalculateNextCrossing();

        levelSlotsList.GetComponent<GenomSlotsCreator>().FillGenome(childGenome);
        levelSlotsList.GetComponent<GenomSlotsCreator>().FillExpectedParents(steps.OrderBy(elem => elem.Item1).Select(elem => elem.Item3).ToList());
    }

    void CalculateNextCrossing()
    {
        beginIndex = rnd.Next(0, 7);
        segmentLength = rnd.Next(2, 9 - beginIndex - 1);

        parent1Genome = parent1StaticsList.GetComponent<GenomCreator>().genomeList;
        parent2Genome = parent2StaticsList.GetComponent<GenomCreator>().genomeList;

        LabeledRecordedList<int, int> recordedCrossing = new(childGenome);
        childGenome = CrosserPartiallyMatched.Cross(parent1Genome, parent2Genome, beginIndex, segmentLength, ref recordedCrossing);
        steps = recordedCrossing.GetFullHistory().Distinct().ToList();

        startingIndexTextContainer.text = $"Starting index: {beginIndex}";
        segmentLengthTextContainer.text = $"Segment length: {segmentLength}";
    }

    public void CreateTutorial()
    {
        tutorialHandler.DestroyGrid(parent1StaticsList, parent1DropsList, parent2StaticsList,  parent2DropsList, levelSlotsList);

        parent1tutorialDropsList.GetComponent<GenomCreator>().CreateNewGenom();
        parent1tutorialStaticsList.GetComponent<GenomCreator>().CreateNewGenom(); 

        parent2tutorialDropsList.GetComponent<GenomCreator>().CreateNewGenom();
        parent2tutorialStaticsList.GetComponent<GenomCreator>().CreateNewGenom();

        tutorialSlotsList.GetComponent<GenomSlotsCreator>().CreateSlots(childGenome);

        tutorialHandler.ActivateTutorial();
    }

    public void LeaveTutorial()
    {
        tutorialHandler.DestroyGrid(parent1tutorialDropsList, parent1tutorialStaticsList, parent2tutorialDropsList, parent2tutorialStaticsList, tutorialSlotsList);

        parent1StaticsList.GetComponent<GenomCreator>().InitializeGenome();
        parent1StaticsList.GetComponent<GenomCreator>().CreateNewGenom();
        parent1DropsList.GetComponent<GenomCreator>().CreateNewGenom();
        
        parent2StaticsList.GetComponent<GenomCreator>().InitializeGenome();
        parent2StaticsList.GetComponent<GenomCreator>().CreateNewGenom();
        parent2DropsList.GetComponent<GenomCreator>().CreateNewGenom();

        CalculateNextCrossing();
        levelSlotsList.GetComponent<GenomSlotsCreator>().CreateSlots(childGenome);
        levelSlotsList.GetComponent<GenomSlotsCreator>().FillExpectedParents(steps.OrderBy(elem => elem.Item1).Select(elem => elem.Item3).ToList());


        tutorialHandler.DeactivateTutorial();
        currentStep = 0;

    }

    public void NextStep()
    {
        if (currentStep < steps.Count)
        {
            var slots = tutorialSlotsList.GetComponent<GenomSlotsCreator>().geneList;
            List<GameObject> drops;
            List<GameObject> statics;
            var (slot, value, parent) = steps[currentStep];
            bool dropIsAboveSlot;
            if (parent == 0)
            {
                drops = parent1tutorialDropsList.GetComponent<GenomCreator>().geneList;
                statics = parent1tutorialStaticsList.GetComponent<GenomCreator>().geneList;
                dropIsAboveSlot = true;
            }
            else
            {
                dropIsAboveSlot = false;
                drops = parent2tutorialDropsList.GetComponent<GenomCreator>().geneList;
                statics = parent2tutorialStaticsList.GetComponent<GenomCreator>().geneList;
            }
            tutorialHandler.Next(value, slot, slots, drops, statics, dropIsAboveSlot);

            currentStep += 1;
            tutorialHandler.ColorCells(parent1tutorialStaticsList.GetComponent<GenomCreator>().geneList, parent2tutorialStaticsList.GetComponent<GenomCreator>().geneList, slots);
  
        }
    }

    public void PreviousStep()
    {
        if (currentStep > 0)
        {
            currentStep -= 1;
            var (_, _, parent) = steps[currentStep];
            bool dropIsAboveSlot = (parent == 0);
            tutorialHandler.Previous(dropIsAboveSlot);
            tutorialHandler.ColorCells(parent1tutorialStaticsList.GetComponent<GenomCreator>().geneList, parent2tutorialStaticsList.GetComponent<GenomCreator>().geneList, tutorialSlotsList.GetComponent<GenomSlotsCreator>().geneList);

        }
    }

    public void HandleAreAllCorrectWithParents()
    {
        tutorialHandler.checkButton.GetComponent<ErrorColoring>().HandleAreAllCorrect();
        List<int> parentList = steps.Select(elem => elem.Item3).ToList();

    }
}
