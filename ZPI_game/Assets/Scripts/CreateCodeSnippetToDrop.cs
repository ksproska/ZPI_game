using System.Collections.Generic;
using UnityEngine;

public class CreateCodeSnippetToDrop : MonoBehaviour
{
    [SerializeField] private GameObject toDropPrefab;
    [SerializeField] private List<string> contents;

    public void Start()
    {
        for (int i = 0; i < contents.Count; i++)
        {
            var added = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity, FindObjectOfType<Canvas>().transform);
            var dd = added.GetComponent<DragDrop>();
            dd.SetContent(contents[i]);
            added.transform.position += Vector3.down * i * 60;
        }
    }
}
