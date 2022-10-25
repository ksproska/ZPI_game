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

    private List<GameObject> allTutorialSlots = new();
    private List<GameObject> allTutorialStatics = new();
    private List<GameObject> allTutorialDrops = new();

    public void Start()
    {
        GameObject levelCanvas = GameObject.Find("LevelCanvas"); 
        for (int i = 0; i < 10; i++)
        {
            var slot = Instantiate(dropSlot, gameObject.transform.position, Quaternion.identity,
                levelCanvas.transform);
            slot.transform.position += new Vector3(300 + i * 121, -120 * 2, 0);
            DropSlot ds = slot.GetComponent<DropSlot>();
            ds.expectedContents = $"{i}";

            var staticAdded = Instantiate(staticPrefab, gameObject.transform.position, Quaternion.identity,
                levelCanvas.transform);
            var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
            staticDd.text = $"{i}";
            staticAdded.transform.position += new Vector3(300 + i * 121, 120 * 2, 0);
        }
        for (int i = 0; i < 10; i++)
        {

            var drop = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity,
                levelCanvas.transform);
            var dd = drop.GetComponent<DragDrop>();
            dd.SetContent($"{i}");
            drop.transform.position += new Vector3(300 + i * 121, 120 * 2, 0);

        }
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
            slot.transform.position += new Vector3(300 + i * 121, -120 * 2 , 0);
            DropSlot ds = slot.GetComponent<DropSlot>();
            ds.expectedContents = $"{i}";
            allTutorialSlots.Add(slot);

            var staticAdded = Instantiate(staticPrefab, gameObject.transform.position, Quaternion.identity,
                tutorialCanvas.transform);
            var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
            staticDd.text = $"{i}";
            staticAdded.transform.position += new Vector3(300 + i * 121, 120 * 2, 0);
            allTutorialStatics.Add(staticAdded);
        }
        for (int i = 0; i < 10; i++)
        {

            var drop = Instantiate(staticPrefab, gameObject.transform.position, Quaternion.identity,
                tutorialCanvas.transform);
            var dd = drop.GetComponent<TextMeshProUGUI>();
            dd.text = $"{i}";
            drop.transform.position += new Vector3(300 + i * 121, 120 * 2, 0);
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
