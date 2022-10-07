using System;
using System.Collections.Generic;
using System.Linq;

namespace GA
{
    public class SelectorRoulette : ISelector
    {
        private static readonly Random Random = new Random();

        public static List<double> GetDistributedWeights(List<Individual> generation)
        {
            var max = generation.Select(i => i.Score).Max();
            var reversedEval = generation.Select(i => (max - i.Score + 1));
            var sum = reversedEval.Sum();
            var evalPercentage = reversedEval.Select(i => i / sum).ToList();

            for (int i = 0; i < evalPercentage.Count - 1; i++)
            {
                evalPercentage[i + 1] += evalPercentage[i];
            }

            return evalPercentage;
        }

        public static int GetIndexForValue(List<double> distributedWeights, double value) {
            for (int i = 0; i < distributedWeights.Count; i++)
            {
                if (value <= distributedWeights[i])
                {
                    return i;
                }
            }
            throw new InvalidOperationException();
        }

        public Individual Get(List<Individual> generation)
        {
            var randDouble = Random.NextDouble();
            var distributedWeights = GetDistributedWeights(generation);
            var index = GetIndexForValue(distributedWeights, randDouble);
            return generation[index];            
        }

        public void SetArgs(params double[] parameters) {}

        public string[] GetArgs()
        {
            return new string[] { }; 
        }
    }
}