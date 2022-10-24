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
    public void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            var slot = Instantiate(dropSlot, gameObject.transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            slot.transform.position += new Vector3(500 + i * 120, -120, 0);
            DropSlot ds = slot.GetComponent<DropSlot>();
            ds.expectedContents = $"{i}";
            slot.layer = 0;
            allCreatedSlots.Add(slot);

            var staticAdded = Instantiate(staticPrefab, gameObject.transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
            staticDd.text = $"{i}";
            staticAdded.transform.position += new Vector3(500 + i * 120, 0, 0);
            staticAdded.layer = 0;

            var added = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            var dd = added.GetComponent<DragDrop>();
            dd.SetContent($"{i}");
            added.transform.position += new Vector3(500 + i * 120, 0, 0);
            added.layer = 1;
        }
        for (int i = 0; i < 3; i++)
        {

            var added = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            var dd = added.GetComponent<DragDrop>();
            dd.SetContent($"{i}");
            added.transform.position += new Vector3(500 + i * 120, 0, 0);
            added.layer = 1;
        }
    }


    public void CreateTutorial()
    {
        for (int i = 0; i < 10; i++)
        {
            var slot = Instantiate(dropSlot, gameObject.transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            slot.transform.position += Vector3.down * 100;
            slot.transform.position += Vector3.right * i * 100 + new Vector3(500, 0, 0);
            allCreatedSlots.Add(slot);

            var staticAdded = Instantiate(staticPrefab, gameObject.transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
            staticDd.text = $"{i}";
            staticAdded.transform.position += Vector3.right * i * 100 + new Vector3(500, 0, 0);

            var added = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            var dd = added.GetComponent<DragDrop>();
            dd.SetContent($"{i}");
            added.transform.position += Vector3.right * i * 100 + new Vector3(500, 0, 0);
        }
    }
}
