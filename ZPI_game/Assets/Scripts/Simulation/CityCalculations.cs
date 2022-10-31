using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CityCalculations
{
    public static List<List<double>> GetDistances(List<City> allCities)
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
    
    public static List<List<double>> InitialiseDistanceMatrix(int size)
    {
        var matrix = new List<List<double>>();
        for(int i=0; i<size; i++)
        {
            var row = new List<double>(new double[size]);
            matrix.Add(row);
        }
        return matrix;
    }
}
