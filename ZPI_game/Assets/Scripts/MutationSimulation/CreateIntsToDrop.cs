using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Assets.GA.Utils;
using GA.mutations;

public class CreateIntsToDrop : MonoBehaviour
{
    System.Random rnd = new System.Random();

    [SerializeField] private GameObject toDropPrefab;
    [SerializeField] private GameObject staticPrefab;
    [SerializeField] private GameObject dropSlot;
    [SerializeField] private GameObject staticDropSlot;

    [NonSerialized] public List<GameObject> allTutorialSlots = new();
    [NonSerialized] public List<GameObject> allTutorialStatics = new();
    [NonSerialized] public List<GameObject> allTutorialDrops = new();

    private int beginIndex;
    private int endIndex;
    private List<int> beginGenome; 
    private List<int> endGenome;
    [NonSerialized] public List<(int, int)> steps;

    public void Start()
    {
        InitializeMutationList();

        
        


        GameObject levelCanvas = GameObject.Find("LevelCanvas"); 
        for (int i = 0; i < 10; i++)
        {
            var slot = Instantiate(dropSlot, gameObject.transform.position, Quaternion.identity,
                levelCanvas.transform);
            slot.transform.position += new Vector3(300 + i * 125, -120 * 2, 0);
            DropSlot ds = slot.GetComponent<DropSlot>();
            ds.expectedContents = $"{beginGenome[i]}";

            var staticAdded = Instantiate(staticPrefab, gameObject.transform.position, Quaternion.identity,
                levelCanvas.transform);
            var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
            staticDd.text = $"{beginGenome[i]}";
            staticAdded.transform.position += new Vector3(300 + i * 125, 120 * 2, 0);
        }
        for (int i = 0; i < 10; i++)
        {

            var drop = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity,
                levelCanvas.transform);
            var dd = drop.GetComponent<DragDrop>();
            dd.SetContent($"{beginGenome[i]}");
            drop.transform.position += new Vector3(300 + i * 125, 120 * 2, 0);

        }
    }

    private void InitializeMutationList()
    {
        beginIndex = rnd.Next(0, 9);
        do
        {
            endIndex = rnd.Next(0, 9);
        } while (endIndex == beginIndex);

        beginGenome = Enumerable.Range(0, 10).OrderBy(elem => rnd.Next()).ToList();
        RecordedList<int> recordedMutation = new(beginGenome);
        endGenome = MutatorPartialReverser<int>.ReversePartOrder(beginGenome, beginIndex, endIndex, ref recordedMutation);
        steps = recordedMutation.GetFullHistory();
    }


    public void CreateTutorial(GameObject tutorialCanvas)
    {
        allTutorialSlots = new();
        allTutorialStatics = new();
        allTutorialDrops = new();

        for (int i = 0; i < 10; i++)
        {
            var slot = Instantiate(dropSlot, gameObject.transform.position, Quaternion.identity,
                tutorialCanvas.transform);
            slot.transform.position += new Vector3(300 + i * 125, -120 * 2 , 0);
            DropSlot ds = slot.GetComponent<DropSlot>();
            ds.expectedContents = $"{endGenome[i]}";
            allTutorialSlots.Add(slot);

            var staticAdded = Instantiate(staticPrefab, gameObject.transform.position, Quaternion.identity,
                tutorialCanvas.transform);
            var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
            staticDd.text = $"{beginGenome[i]}";
            staticAdded.transform.position += new Vector3(300 + i * 125, 120 * 2, 0);
            allTutorialStatics.Add(staticAdded);
        }
        for (int i = 0; i < 10; i++)
        {

            var drop = Instantiate(staticDropSlot, gameObject.transform.position, Quaternion.identity,
                tutorialCanvas.transform);
            var dd = drop.GetComponent<TextMeshProUGUI>();
            dd.text = $"{beginGenome[i]}";
            drop.transform.position += new Vector3(300 + i * 125, 120 * 2, 0);
            allTutorialDrops.Add(drop);

        }
    }

    public void DestroyTutorial()
    {
        for (int i = 0; i < 10; i++)
        {
            Destroy(allTutorialSlots[i]);
            Destroy(allTutorialStatics[i]);
            Destroy(allTutorialDrops[i]);
        }
    }
}
