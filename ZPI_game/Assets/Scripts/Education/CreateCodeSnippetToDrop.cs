using System.Collections.Generic;
using UnityEngine;

public class CreateCodeSnippetToDrop : MonoBehaviour
{
    [SerializeField] private GameObject toDropPrefab;
    [SerializeField] private List<string> contents;
    [SerializeField] private int customPixelDistance = -1;

    public void Start()
    {
        var pixelDistance = customPixelDistance  == -1 ? 45 : customPixelDistance;
        for (int i = 0; i < contents.Count; i++)
        {
            var added = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity, transform);
            var dd = added.GetComponent<EducationDragDrop>();
            dd.SetContent(contents[i]);
            added.transform.position += Vector3.down * i * pixelDistance;
        }
    }
}
