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
    IGeneticAlgorithm<int> ga;
    private float lastUpdate = 0;
    private List<City> allCities;
    
    void Start()
    {
        //GetAllCities().ForEach(c => Debug.Log(c.GetPosition()));
        //Debug.Log(GetMatrixPretty(GetDistances()));
        allCities = GetAllCities();
        ga = GeneticAlgorithmProxy.Get(
            GetDistances(),
            20,
            MutationType.PartialReverser,
            0.2d,
            CrossoverType.PartiallyMatched,
            0.7d,
            SelectionType.Tournament,
            0.5);
    }

    
    void Update()
    {
        lastUpdate += Time.deltaTime;
        if(lastUpdate > 2)
        {
            lastUpdate = 0;
            ga.RunIteration();
            int iterNumber = ga.GetIterationNumber();
            double bestScore = ga.GetBestScore();
            double bestForIter = ga.GetBestForIterationScore();
            textBox.text = $"Iteration #{iterNumber}\nBest for iteration: {bestForIter:0.000}\nBest all time: {bestScore:0.000}";
            var bestGenome = ga.GetBestGenotype();
            AssignCityOrderToDisplay(bestGenome, allCities);
            DrawLines(bestGenome);
        }
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
        var cityList = GetAllCities();
        var distanceMatrix = InitialiseDistanceMatrix(cityList.Count);
        for(int i = 0; i < cityList.Count; i++)
        {
            for(int j = i + 1; j < cityList.Count; j++)
            {
                distanceMatrix[i][j] = cityList[i].Distance(cityList[j]);
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
        //genome.Select((value, index) => (value, index)).ToList().ForEach(pair => 
        //{
        //    var (value, index) = pair;
        //    City current = allCities.Find(city => city.cityNumber == value);
        //    int nextIndex = (index + 1) % genome.Count;
        //    City next = allCities.Find(city => city.cityNumber == genome[nextIndex]);
        //    Debug.DrawLine(current.transform.position, next.transform.position, Color.red, 2f);
        //});

        foreach (var (value, index) in genome.Select((value, index) => (value, index)))
        {
            City current = allCities.Find(city => city.cityNumber == value);
            int nextIndex = (index + 1) % genome.Count;
            City next = allCities.Find(city => city.cityNumber == genome[nextIndex]);
            Debug.DrawLine(current.transform.position, next.transform.position, Color.red, 2f);
        }
    }
}
