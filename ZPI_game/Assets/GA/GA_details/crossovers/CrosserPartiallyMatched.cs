using System;
using System.Collections.Generic;
using System.Linq;

namespace GA
{
    public class CrosserPartiallyMatched : ICrosser<int>
    {
        private static readonly Random Random = new Random();
        public static List<int> PartiallyMatchedCrossover(List<int> parent1, List<int> parent2, int startInx, int segmentLength)
        {
            if (startInx + segmentLength >= parent1.Count) //TODO ugly
            {
                segmentLength = parent1.Count - 1 - startInx;
            }
            var child = Enumerable.Repeat(-1, parent1.Count).ToList(); //TODO change int -> T (generic)
            for (int i = 0; i < segmentLength; i++) {
                int inx = i + startInx;
                child[inx] = parent1[inx];
            }
            for (int i = 0; i < segmentLength; i++) {
                int inx = i + startInx;
                int valToAdd = parent2[inx];
    
                if(!child.Contains(valToAdd)) {
                    while (child[inx] != -1)
                    {
                        inx = parent2.IndexOf(parent1[inx]);
                    }
                    child[inx] = valToAdd;
                }
            }
            for (int i = 0; i < child.Count; i++) {
                if(child[i] == -1) {
                    child[i] = parent2[i];
                }
            }
            return child;
        }
        public List<int> Get(List<int> parent1, List<int> parent2)
        {
            var startInx = Random.Next(parent1.Count);
            var segmentLen = Random.Next(parent1.Count);
            return PartiallyMatchedCrossover(parent1, parent2, startInx, segmentLen);
        }
    }
}