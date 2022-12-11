using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour
{
    public ErrorColoring dropHandler;

    public GameObject levelButton;
    public GameObject tutorialButton;
    public GameObject checkButton;



    public GameObject tutorialContainer;
    public GameObject levelContainer;

    public GameObject nextButton;
    public GameObject previousButton;

    private Stack<GameObject> tutorialStack;

    [NonSerialized] public GameObject slider;
    [NonSerialized] public GameObject target;


    [NonSerialized] public List<(int, int)> steps;

    [SerializeField] private List<Color> colors;
    private Stack<(GameObject, GameObject)> colorStack;

    public enum mode
    {
        Previous,
        Next
    }

    // Start is called before the first frame update
    void Start()
    {   
        tutorialStack = new();
        colorStack = new();
    }

    // Update is called once per frame
    void FixedUpdate()
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
                levelButton.GetComponent<Button>().enabled = true;
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
    public void DestroyGrid(params GameObject[] args)
    {
        foreach (var list in args)
        {
            foreach (Transform child in list.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public void ActivateTutorial()
    {
        levelContainer.SetActive(false);
        tutorialContainer.SetActive(true);
        tutorialButton.SetActive(false);
        checkButton.SetActive(false);
        levelButton.SetActive(true);
        nextButton.SetActive(true);
        previousButton.SetActive(true);
        tutorialStack = new();
        colorStack = new();
    }

    public void DeactivateTutorial()
    {
        levelContainer.SetActive(true);
        tutorialContainer.SetActive(false);
        tutorialButton.SetActive(true);
        checkButton.SetActive(true);
        levelButton.SetActive(false);
        nextButton.SetActive(false);
        previousButton.SetActive(false);
    }

    public void ColorCells(params List<GameObject>[] args)
    {
        Stack<(GameObject, GameObject)> cells = new Stack<(GameObject, GameObject)>(new Stack<(GameObject, GameObject)>(colorStack));
        int previousCells = Math.Min(3, cells.Count);

        foreach (var list in args)
        {
            foreach (var elem in list)
            {
                var image = elem.GetComponentInChildren<Image>();
                var tempColor = Color.black;
                if (elem.GetComponent<GenomCreator>())
                {
                    tempColor.a = 0.0f;
                }
                else
                {
                    tempColor.a = 0.4f;
                }
                image.color = tempColor;
            }
        }

        for (int i = 0; i < previousCells; i++)
        {
            var (slot, staticElem) = cells.Pop();
            staticElem.GetComponentInChildren<Image>().color = colors[2-i];
            slot.GetComponentInChildren<Image>().color = colors[2-i];
        }
    }

    public void Next(int dropNumber, int slotIndex, List<GameObject> slots, List<GameObject> drops,List<GameObject> statics, bool dropIsAboveSlot)
    {
        previousButton.GetComponent<Button>().enabled = false;
        nextButton.GetComponent<Button>().enabled = false;
        levelButton.GetComponent<Button>().enabled = false;
        var drop = drops.Where(item => item.GetComponent<TextMeshProUGUI>().text == $"{dropNumber}").First();
        var staticElem = statics.Where(item => item.GetComponent<TextMeshProUGUI>().text == $"{dropNumber}").First();
        LineRenderer lineRenderer = drop.GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        Vector3[] pathPoints;
        if (dropIsAboveSlot)
        {
            pathPoints = new Vector3[2] { drop.transform.position - new Vector3(0, 0.63f), slots[slotIndex].transform.position + new Vector3(0, 0.63f) };
        }
        else
        {
            pathPoints = new Vector3[2] { drop.transform.position + new Vector3(0, 0.63f), slots[slotIndex].transform.position - new Vector3(0, 0.63f) };
        }
        lineRenderer.SetPositions(pathPoints);
        slider = drop;
        target = slots[slotIndex];

        tutorialStack.Push(drop);
        colorStack.Push((slots[slotIndex], staticElem));
        
    }

    public void Previous(bool dropIsAboveSlot)
    {
        previousButton.GetComponent<Button>().enabled = false;
        nextButton.GetComponent<Button>().enabled = false;
        var drop = tutorialStack.Pop();
        var (_, staticElem) = colorStack.Pop();
        LineRenderer lineRenderer = drop.GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        Vector3[] pathPoints;
        if (dropIsAboveSlot)
        {
            pathPoints = new Vector3[2] { drop.transform.position + new Vector3(0, 0.63f), staticElem.transform.position - new Vector3(0, 0.63f) };
        }
        else
        {
            pathPoints = new Vector3[2] { drop.transform.position - new Vector3(0, 0.63f), staticElem.transform.position + new Vector3(0, 0.63f) };
        }
        lineRenderer.SetPositions(pathPoints);

        slider = drop;
        target = staticElem;
    }
}
