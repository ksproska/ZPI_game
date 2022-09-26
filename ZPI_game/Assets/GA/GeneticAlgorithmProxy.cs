using System;
using System.Collections.Generic;

namespace GA
{
    public abstract class GeneticAlgorithmProxy
    {
        public static IGeneticAlgorithm<int> Get(
            List<List<double>> weights, int generationSize,
            MutationType mutationType, double mutationProbability, 
            CrossoverType crossoverType, double crossoverProbability,
            SelectionType selectionType, params double[] selectionArgs
            )
        {
            Console.WriteLine(selectionArgs.Length);
            var grid = new WeightsGrid(weights);
            var mutation = FactoryMutation.Get<int>(mutationType);
            var crossover = FactoryCrossover.Get(crossoverType);
            var selector = FactorySelection.Get<int>(selectionType, selectionArgs);

            return new GeneticAlgorithm(grid, generationSize, 
                selector,
                mutation, mutationProbability,
                crossover, crossoverProbability
            );
        }
    }
}