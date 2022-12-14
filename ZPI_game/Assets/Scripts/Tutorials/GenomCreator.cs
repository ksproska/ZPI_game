using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class GenomCreator : MonoBehaviour
{
    readonly System.Random rnd = new();

    [SerializeField] private GameObject prefab;
    [SerializeField] private bool initializeGenome ;
    public bool initializeGrid;

    [NonSerialized] public List<int> genomeList;
    [NonSerialized] public List<GameObject> geneList = new();
    public GenomCreator otherGenome;
    public int parentNumber;

    private void Awake()
    {
        if (initializeGenome)
        {
            InitializeGenome();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (initializeGrid)
        {
            CreateNewGenom();
        }
    }

    public void CreateNewGenom()
    {

        geneList = new();
        for (int i = 0; i < 10; i++)
        {
            var staticAdded = Instantiate(prefab, gameObject.transform.position, Quaternion.identity,
                transform);
            staticAdded.transform.position += new Vector3(i * 1.25f, 0, 0);
            var index = staticAdded.GetComponentInChildren<Text>();
            if (index != null) {
                index.text = $"{i}";
            }
            if (initializeGenome)
            {
                var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
                staticDd.text = $"{genomeList[i]}";
                var dragDrop = staticAdded.GetComponent<DragDrop>();
                if (dragDrop)
                {
                    dragDrop.parent = parentNumber;
                }
            }
            else if (otherGenome != null)
            {
                var staticDd = staticAdded.GetComponent<TextMeshProUGUI>();
                staticDd.text = $"{otherGenome.genomeList[i]}";
                var dragDrop = staticAdded.GetComponent<DragDrop>();
                if (dragDrop)
                {
                    dragDrop.parent = otherGenome.parentNumber;
                }
            }
            geneList.Add(staticAdded);
        }
    }

    public void InitializeGenome()
    {
        genomeList = Enumerable.Range(0, 10).OrderBy(elem => rnd.Next(0,10)).ToList();
    }

    public void FillGenome(List<int> input)
    {
        for(int i = 0; i< genomeList.Count; i++)
        {
            geneList[i].GetComponent<TextMeshProUGUI>().text = $"{input[i]}";
        }
    }

    public void ReloadGenome()
    {
        if (otherGenome)
        {
            genomeList = otherGenome.genomeList;
        }
        FillGenome(genomeList);
    }

}
