using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Assets.GA.Utils;
using GA.mutations;

public class MutationTutorialController : MonoBehaviour
{
    System.Random rnd = new System.Random();


    public TutorialHandler tutorialHandler;

    public TextMeshProUGUI StartingIndexTextContainer;
    public TextMeshProUGUI EndingIndexTextContainer;
    public TextMeshProUGUI IndicesListTextContainer;



    public GameObject tutorialContainer;
    public GameObject levelContainer;

    public GameObject slotListPrefab;
    public GameObject dropsListPrefab;
    public GameObject staticsListPrefab;

    public GameObject tutorialSlotsList;
    public GameObject tutorialDropsList;
    public GameObject tutorialStaticsList;



    private int currentStep;



    [NonSerialized] public int beginIndex;
    [NonSerialized] public int endIndex;
    [NonSerialized] public List<int> indices;
    [NonSerialized] public List<int> beginGenome;
    [NonSerialized] public List<int> endGenome;
    [NonSerialized] public List<(int, int)> steps;

    public string mutationType;

    // Start is called before the first frame update
    void Start()
    {
        currentStep = 0;
        CalculateNextCrossing();

        slotListPrefab.GetComponent<GenomSlotsCreator>().FillGenome(endGenome);
    }

    void CalculateNextCrossing()
    {
        beginGenome = staticsListPrefab.GetComponent<GenomCreator>().genomeList;
        RecordedList<int> recordedMutation = new(beginGenome);
        if(mutationType == "rsm")
        {
            beginIndex = rnd.Next(0, 9);
            do
            {
                endIndex = rnd.Next(0, 9);
            } while (endIndex - beginIndex == 0 || endIndex - beginIndex == -1);

            endGenome = MutatorReverseSequence<int>.ReversePartOrder(beginGenome, beginIndex, endIndex, ref recordedMutation);
            steps = recordedMutation.GetFullHistory().Distinct().ToList();

            StartingIndexTextContainer.text = $"Starting index: {beginIndex}";
            EndingIndexTextContainer.text = $"Ending index: {endIndex}";
        }
        else if(mutationType == "thrors")
        {
            indices = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            indices = indices.OrderBy(x => rnd.Next()).Take(rnd.Next(1, 8)).ToList();

            endGenome = MutatorThrors<int>.SwipeOneRight(beginGenome, indices, ref recordedMutation);
            steps = recordedMutation.GetFullHistory().Distinct().ToList();

            IndicesListTextContainer.text = $"Indices to swipe: ";
            foreach(var index in indices)
            {
                IndicesListTextContainer.text += $"{index}  ";
            }
        }

    }


    public void CreateTutorial()
    {
        tutorialHandler.DestroyGrid(staticsListPrefab, dropsListPrefab, slotListPrefab);

        tutorialDropsList.GetComponent<GenomCreator>().CreateNewGenom();
        tutorialStaticsList.GetComponent<GenomCreator>().CreateNewGenom();
        tutorialSlotsList.GetComponent<GenomSlotsCreator>().CreateSlots(endGenome);

        tutorialHandler.ActivateTutorial();
    }

    public void LeaveTutorial()
    {
        tutorialHandler.DestroyGrid(tutorialDropsList, tutorialStaticsList, tutorialSlotsList);

        staticsListPrefab.GetComponent<GenomCreator>().InitializeGenome();
        staticsListPrefab.GetComponent<GenomCreator>().CreateNewGenom();
        dropsListPrefab.GetComponent<GenomCreator>().CreateNewGenom();

        CalculateNextCrossing();
        slotListPrefab.GetComponent<GenomSlotsCreator>().CreateSlots(endGenome);

        tutorialHandler.DeactivateTutorial();
        currentStep = 0;
    }

    public void NextStep()
    {
        if(currentStep < steps.Count)
        {
            var slots = tutorialSlotsList.GetComponent<GenomSlotsCreator>().geneList;
            var drops = tutorialDropsList.GetComponent<GenomCreator>().geneList;
            var statics = tutorialStaticsList.GetComponent<GenomCreator>().geneList;
            var (slot, value) = steps[currentStep];
            tutorialHandler.Next(value, slot, slots, drops, statics, true);
            tutorialHandler.ColorCells(slots, statics);
            currentStep += 1;

        }
    }

    public void PreviousStep()
    {
        if (currentStep > 0)
        {
            
            var statics = tutorialStaticsList.GetComponent<GenomCreator>().geneList;
            var slots = tutorialSlotsList.GetComponent<GenomSlotsCreator>().geneList;
            tutorialHandler.Previous(true);
            tutorialHandler.ColorCells(slots, statics);
            //drop.transform.position = singleStatic.transform.position;
            currentStep -= 1;
        }
    }
}
