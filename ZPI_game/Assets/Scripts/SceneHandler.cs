using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    
    void Start()
    {
        GetAllCities().ForEach(c => Debug.Log(c.GetPosition()));
        GetDistances().ForEach(row => row.ForEach(dis => Debug.Log(dis)));
    }

    
    void Update()
    {
        
    }

    public List<City> GetAllCities()
    {
        var list = new List<City>(FindObjectsOfType<City>());
        return list;
    }

    public List<List<float>> GetDistances()
    {
        var cityList = GetAllCities();

        float[,] distances = new float[cityList.Count, cityList.Count];
        for(int i = 0; i < cityList.Count; i++)
        {
            for(int j = i + 1; j < cityList.Count; j++)
            {
                distances[i, j] = cityList[i].Distance(cityList[j]);
                distances[j, i] = distances[i, j];
            }
        }

        var ret = new List<List<float>>();
        for(int i = 0; i < distances.GetLength(0); i++)
        {
            var row = new List<float>();
            for(int j = 0; j < distances.GetLength(1); j++)
            {
                row.Add(distances[i, j]);
            }
            ret.Add(row);
        }
        return ret;
    }
}
