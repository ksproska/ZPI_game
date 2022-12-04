using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DeveloperUtils;
using TMPro;
using UnityEngine.UI;

public class GenomSlotsCreator : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [NonSerialized] public List<int> valueList;
    [NonSerialized] public List<GameObject> geneList = new();
    public bool initialize;

    private void Awake()
    {
        if (initialize)
        {
            for (int i = 0; i < 10; i++)
            {
                var staticAdded = Instantiate(prefab, gameObject.transform.position, Quaternion.identity,
                    transform);
                var index = staticAdded.GetComponentInChildren<Text>();
                index.text = $"{i}";
                staticAdded.transform.position += new Vector3(i * 1.25f, 0, 0);
                geneList.Add(staticAdded);
            }
        }
    }

    void Start()
    {
        // if (initialize)
        // {
        //     for (int i = 0; i < 10; i++)
        //     {
        //         var staticAdded = Instantiate(prefab, gameObject.transform.position, Quaternion.identity,
        //             transform);
        //         var index = staticAdded.GetComponentInChildren<Text>();
        //         index.text = $"{i}";
        //         staticAdded.transform.position += new Vector3(i * 1.25f, 0, 0);
        //         geneList.Add(staticAdded);
        //     }
        // }
    }

    public void CreateSlots(List<int> input)
    {
        geneList = new();
        for (int i = 0; i < 10; i++)
        {
            var staticAdded = Instantiate(prefab, gameObject.transform.position, Quaternion.identity,
                transform);
            var index = staticAdded.GetComponentInChildren<Text>();
            index.text = $"{i}";
            staticAdded.transform.position += new Vector3(i * 1.25f, 0, 0);
            staticAdded.GetComponent<DropSlot>().expectedContents = $"{input[i]}";
            geneList.Add(staticAdded);
        }
    }

    public void FillExpectedParents(List<int> parents)
    {
        for (int i = 0; i < geneList.Count; i++)
        {
            geneList[i].GetComponent<DropSlot>().expectedParent = parents[i];
        }
    }


    public void FillGenome(List<int> input)
    {
        for (int i = 0; i < geneList.Count; i++)
        {
            geneList[i].GetComponent<DropSlot>().expectedContents = $"{input[i]}";
            geneList[i].GetComponent<DropSlot>()._placedContent = $"";
        }
    }


}
