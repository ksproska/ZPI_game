using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateIntsToDrop : MonoBehaviour
{
    [SerializeField] private GameObject toDropPrefab;
    [SerializeField] private GameObject staticPrefab;
    [SerializeField] private GameObject dropSlot;

    private List<GameObject> allCreatedSlots = new List<GameObject>();
    private List<GameObject> allCreatedStatics = new List<GameObject>();
    private List<GameObject> allCreatedDrops = new List<GameObject>();
    public void Start()
    {
        GameObject levelCanvas = GameObject.Find("LevelCanvas"); 
        for (int i = 0; i < 10; i++)
        {
            var slot = Instantiate(dropSlot, gameObject.transform.position, Quaternion.identity,
                levelCanvas.transform);
            slot.transform.position += new Vector3(500 + i * 120, -120, 0);
            DropSlot ds = slot.GetComponent<DropSlot>();
            ds.expectedContents = $"{i}";
            allCreatedSlots.Add(slot);

            var staticAdded = Instantiate(staticPrefab, gameObject.transform.position, Quaternion.identity,
                levelCanvas.transform);
            var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
            staticDd.text = $"{i}";
            staticAdded.transform.position += new Vector3(500 + i * 120, 0, 0);
            allCreatedStatics.Add(staticAdded);
        }
        for (int i = 0; i < 10; i++)
        {

            var drop = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity,
                levelCanvas.transform);
            var dd = drop.GetComponent<DragDrop>();
            dd.SetContent($"{i}");
            drop.transform.position += new Vector3(500 + i * 120, 0, 0);
            allCreatedDrops.Add(drop);

        }
    }


    public void CreateTutorial()
    {
        GameObject.Find("LevelCanvas").SetActive(false);
        GameObject tutorialCanvas = GameObject.Find("TutorialCanvas");
        tutorialCanvas.SetActive(true);

        List<GameObject> allTutorialSlots = new();
        List<GameObject> allTutorialStatics = new();
        List<GameObject> allTutorialDrops = new();

        for (int i = 0; i < 10; i++)
        {
            var slot = Instantiate(dropSlot, gameObject.transform.position, Quaternion.identity,
                tutorialCanvas.transform);
            slot.transform.position += new Vector3(500 + i * 120, -120, 0);
            DropSlot ds = slot.GetComponent<DropSlot>();
            ds.expectedContents = $"{i}";
            allTutorialSlots.Add(slot);

            var staticAdded = Instantiate(staticPrefab, gameObject.transform.position, Quaternion.identity,
                tutorialCanvas.transform);
            var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
            staticDd.text = $"{i}";
            staticAdded.transform.position += new Vector3(500 + i * 120, 0, 0);
            allTutorialStatics.Add(staticAdded);
        }
        for (int i = 0; i < 10; i++)
        {

            var drop = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity,
                tutorialCanvas.transform);
            var dd = drop.GetComponent<DragDrop>();
            dd.SetContent($"{i}");
            drop.transform.position += new Vector3(500 + i * 120, 0, 0);
            allTutorialDrops.Add(drop);

        }
    }
}
