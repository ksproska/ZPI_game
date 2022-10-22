using System;
using System.Collections.Generic;
using System.Linq;

namespace GA
{
    public class Individual // Można dodać interfejs z metodami GetRandom, GetBest, bo sam Individual może być różnie implementowany
    {
        // nazewnictwo zmiennych niezgodne z konwencją
        public readonly List<int> Genotype;
        public readonly double Score;
        private static readonly Random Random = new Random();
        
        public Individual(List<int> genotype, WeightsGrid weightsGrid)
        {
            Genotype = genotype;
            Score = GetScore(genotype, weightsGrid);
        }
        
        public static double GetScore(List<int> genotype, WeightsGrid weightsGrid)
        {
            var sum = 0.0;
            for (int i = 0; i < genotype.Count; i++)
            {
                var j = (i + 1) % genotype.Count;
                sum += weightsGrid.GetWeight(genotype[i], genotype[j]);
            }
            return sum;
        }

        public Individual(double score)
        {
            Genotype = new List<int>();
            Score = score;
        }

        public static Individual GetRandom(WeightsGrid weightsGrid)
        {
            var randomGenotype = Enumerable.Range(0, weightsGrid.Size).ToList();
            Shuffle(randomGenotype);
            return new Individual(randomGenotype, weightsGrid);
        }

        public static Individual GetBest(List<Individual> members)
        {
            var bestScore = double.PositiveInfinity;
            Individual best = null;
            foreach (var member in members)
            {
                if (member.Score < bestScore)
                {
                    bestScore = member.Score;
                    best = member;
                }
            }
            return best;
        }

        private static void Shuffle<T>(IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = Random.Next(n + 1);  
                (list[k], list[n]) = (list[n], list[k]);
            }  
        }
    }
}
