using System;
using System.Collections.Generic;
using System.Linq;

namespace GA
{
    public abstract class SelectionCommonMethods
    {
        private static readonly Random Random = new Random(); // nazewnictwo zmiennej niezgodne z konwencją
        public static HashSet<int> GetRandom(int max, int len) {
            HashSet<int> selected = new HashSet<int>();
            while (len > selected.Count) {
                selected.Add(Random.Next(max));
            }
            return selected;
        }

        public static List<Individual> GetForIndexes(List<Individual> generation, HashSet<int> indexes)
        {
            var selected = new List<Individual>();
            foreach (var inx in indexes)
            {
                selected.Add(generation[inx]);
            }

            return selected;
        }
    }
}