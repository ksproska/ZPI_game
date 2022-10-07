using System;
using System.Collections.Generic;
using System.Linq;

namespace GA
{
    public class CrosserOrder: ICrosser<int>
    {
        private static readonly Random Random = new Random();
        public static List<int> Cross(List<int> parent1, List<int> parent2, int startInx, int segmentLength)
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

            int lastNotContained = 0;
            for (int i = 0; i < child.Count; i++) {
                if(child[i] == -1) {
                    for (int j = lastNotContained; j < parent2.Count; j++)
                    {
                        lastNotContained = j + 1;
                        if (!child.Contains(parent2[j]))
                        {
                            child[i] = parent2[j];
                            break; //todo ugly
                        }
                    }
                }
            }
            return child;
        }
        public List<int> Get(List<int> parent1, List<int> parent2)
        {
            var startInx = Random.Next(parent1.Count);
            var segmentLen = Random.Next(parent1.Count);
            return Cross(parent1, parent2, startInx, segmentLen);
        }
    }
}