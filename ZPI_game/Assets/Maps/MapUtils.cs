using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using UnityEngine;

namespace Maps
{
    public static class MapUtils
    {
        public static List<List<double>> GetDistanceMatrix(Map map)
        {
            List<List<double>> distMatrix = InitialiseDistanceMatrix(map.Points.Count);
            for(int rowInx = 0; rowInx < map.Points.Count; rowInx++)
            {
                for(int colInx = 0; colInx < map.Points.Count; colInx++)
                {
                    distMatrix[rowInx][colInx] = rowInx == colInx ? 0 : GetDistance(map.Points[rowInx], map.Points[colInx]);
                }
            }
            return distMatrix;
        }
        //public static string MapToJson(Map map)
        //{
        //    return JsonSerializer.Serialize<Map>(map);
        //}


        private static double GetDistance(Point fst, Point snd)
        {
            return Mathf.Sqrt((fst.X - snd.X) * (fst.X - snd.X) + (fst.Y - snd.Y) * (fst.Y - snd.Y));
        }
        private static List<List<double>> InitialiseDistanceMatrix(int size)
        {
            var matrix = new List<List<double>>();
            for (int i = 0; i < size; i++)
            {
                var row = new List<double>(new double[size]);
                matrix.Add(row);
            }
            return matrix;
        }
    }
}

