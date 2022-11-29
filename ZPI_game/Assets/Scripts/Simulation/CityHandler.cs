using Maps;
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
    public static Map CitiesToMap(List<City> cities)
    {
        List<Point> points = cities.Select(city => new Point(city.GetAnchoredPosition().Item1, city.GetAnchoredPosition().Item2)).ToList();
        return new Map(points);
    }

    public static List<City> MapToCity(Map map, GameObject parent, City mockup)
    {
        var ret = new List<City>();
        var parentRectTransform = parent.GetComponent<RectTransform>();
        foreach (var (x, y) in map.Points)
        {
            var city = Instantiate(mockup, parent.transform);
            var pointTransform = city.GetComponent<RectTransform>();
            pointTransform.SetParent(parentRectTransform);
            pointTransform.anchoredPosition = new Vector3(x, y, 0);
            pointTransform.anchorMax = new(0, 0);
            pointTransform.anchorMin = new(0, 0);
            pointTransform.pivot = new Vector2(0.5f, 0.5f);
            ret.Add(city);
        }
        return ret;
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
