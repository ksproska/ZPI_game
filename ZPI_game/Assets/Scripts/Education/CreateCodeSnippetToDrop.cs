using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperUtils;
using UnityEngine;
using Random = System.Random;

public class CreateCodeSnippetToDrop : MonoBehaviour
{
    [SerializeField] private GameObject toDropPrefab;
    [SerializeField] private List<string> contents;

    public void Start()
    {
        var pixelDistance = 45;
        
        // Change the distance for 4k UHD resolution
        if (Math.Abs(Screen.height - 2160d) < 1e-10 && Math.Abs(Screen.width - 3840d) < 1e-10)
        {
            pixelDistance = 90;
        }

        contents = contents.OrderBy(e => new Random().NextDouble()).ToList();
        
        for (int i = 0; i < contents.Count; i++)
        {
            var added = Instantiate(toDropPrefab, gameObject.transform.position, Quaternion.identity, transform);
            var dd = added.GetComponent<EducationDragDrop>();
            dd.SetContent(contents[i]);
            added.transform.position += Vector3.down * i * pixelDistance;
        }
    }
}
