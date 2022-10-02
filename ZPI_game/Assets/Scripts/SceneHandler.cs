using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GA;
using UnityEngine.UI;
using UnityEditor;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] Text textBox;
    [SerializeField] Text history;
    IGeneticAlgorithm<int> ga;
    IGaPrettifier gaPrettifier;
    private float lastUpdate = 0;
    private List<City> allCities;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] float _deltaTime = 0.1f;
    private bool _runAlgoritm = false;
    private Canvas _canvas;
    [SerializeField] private Dropdown selection, crossover, mutation;

    public void SetSpeed(float speed)
    {
        _deltaTime = speed;
    }
    
    void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        allCities = GetAllCities();
        _lineRenderer.positionCount = allCities.Count + 1;
        ga = GeneticAlgorithmProxy.Get(
            GetDistances(),
            20,
            MutationType.PartialReverser,
            0.2d,
            CrossoverType.PartiallyMatched,
            0.7d,
            SelectionType.Tournament,
            0.5);
        gaPrettifier = new GaPrettifier<int>(ga);
        history.text = gaPrettifier.GetIterationLogHeader();
        selection.AddOptions(TypeToNameMappers.GetSelectionDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        crossover.AddOptions(TypeToNameMappers.GetCrossoverDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        mutation.AddOptions(TypeToNameMappers.GetMutationDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
    }

    public void SetSelection()
    {
        Debug.Log(TypeToNameMappers.GetSelectionDescriptionMapper()[selection.options[selection.value].text]);
    }
    public void SetCrossover()
    {
        Debug.Log(TypeToNameMappers.GetCrossoverDescriptionMapper()[crossover.options[crossover.value].text]);
    }
    public void SetMutation()
    {
        Debug.Log(TypeToNameMappers.GetMutationDescriptionMapper()[mutation.options[mutation.value].text]);
    }

    void Update()
    {
        if (_runAlgoritm)
        {
            lastUpdate += Time.deltaTime;
            if(lastUpdate > _deltaTime)
            {
                lastUpdate = 0;
                textBox.text = gaPrettifier.GetCurrentIterationLog();
                history.text += gaPrettifier.GetCurrentIterationLogIfNewBestFound();
                var bestGenome = ga.GetBestGenotype();
                AssignCityOrderToDisplay(bestGenome, allCities);
                DrawLines(bestGenome);
                ga.RunIteration();
            }
        }
    }

    public void setRunning()
    {
        _runAlgoritm = !_runAlgoritm;
    }
    
    public List<City> GetAllCities()
    {
        var list = new List<City>(FindObjectsOfType<City>());
        int number = 0;
        list.ForEach(c => c.cityNumber = number++);
        return list;
    }

    public List<List<double>> GetDistances()
    {
        var distanceMatrix = InitialiseDistanceMatrix(allCities.Count);
        for(int i = 0; i < allCities.Count; i++)
        {
            for(int j = i + 1; j < allCities.Count; j++)
            {
                distanceMatrix[i][j] = allCities[i].Distance(allCities[j]);
                distanceMatrix[j][i] = distanceMatrix[i][j];
            }
        }
        return distanceMatrix;
    }

    private List<List<double>> InitialiseDistanceMatrix(int size)
    {
        var matrix = new List<List<double>>();
        for(int i=0; i<size; i++)
        {
            var row = new List<double>(new double[size]);
            matrix.Add(row);
        }
        return matrix;
    }

    private void AssignCityOrderToDisplay(List<int> genotype, List<City> cities)
    {
        cities.ForEach(city =>
        {
            int index = genotype.FindIndex(num => num == city.cityNumber);
            city.SetText(index.ToString());
        });
    }

    private string GetMatrixPretty(List<List<double>> matrix)
    {
        var ret = "";
        matrix.ForEach(row =>
        {
            row.ForEach(dist =>
            {
                ret += dist.ToString();
                ret += " ";
            });
            ret += "\n";
        });
        return ret;
    }

    private void DrawLines(List<int> genome)
    {
        foreach (var (value, index) in genome.Select((value, index) => (value, index)))
        {
            City current = allCities.Find(city => city.cityNumber == value);
            int nextIndex = (index + 1) % genome.Count;
            City next = allCities.Find(city => city.cityNumber == genome[nextIndex]);
            current.DrawLine(next);
            //_lineRenderer.SetPosition(index, current.GetComponent<RectTransform>().transform.position);
            //_lineRenderer.SetPosition(index+1, next.GetComponent<RectTransform>().transform.position);
        }
    }
}
