using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateIntsToDrop : MonoBehaviour
{
    [SerializeField] private GameObject toDropPrefab;
    [SerializeField] private GameObject dropSlot;
    public void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var added = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            var dd = added.GetComponent<DragDrop>();
            dd.SetContent($"{i}");
            added.transform.position += Vector3.right * i * 100;
                
            added = Instantiate(dropSlot, gameObject.transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            added.transform.position += Vector3.down * 100;
            added.transform.position += Vector3.right * i * 100;
        }
    }
}
