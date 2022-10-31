using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityHandler : MonoBehaviour
{
    public static List<City> GetAllCities()
    {
        var list = new List<City>(FindObjectsOfType<City>());
        int number = 0;
        list.ForEach(c => c.cityNumber = number++);
        return list;
    }
    public static void DrawLines(List<int> genome, List<City> allCities)
    {
        foreach (var (value, index) in genome.Select((value, index) => (value, index)))
        {
            City current = allCities.Find(city => city.cityNumber == value);
            int nextIndex = (index + 1) % genome.Count;
            City next = allCities.Find(city => city.cityNumber == genome[nextIndex]);
            current.DrawLine(next);
        }
    }
}
