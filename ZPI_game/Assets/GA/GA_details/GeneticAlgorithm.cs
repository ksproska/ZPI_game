using System;
using System.Collections.Generic;

namespace GA
{
    public class GeneticAlgorithm : IGeneticAlgorithm<int>
    {
        private static readonly Random Random = new Random();
        
        private readonly WeightsGrid _weightsGrid;
        private List<Individual> _generation;
        public Individual Previous { get; private set; }
        public Individual Best { get; private set; }
        public Individual BestForIteration { get; private set; }
        public int Iteration { get; private set; }
        public int LastIterationWithNewBestDiscovered { get; private set; }
        public int BetweenChangesDiff { get; private set;  }
        public int BetweenChangesDiffPrev { get; private set;  }

        private readonly ISelector _selector;
        private readonly IMutator<int> _mutator;
        private readonly ICrosser<int> _crosser;
        private readonly double _mutationProbability, _crossoverProbability;


        public GeneticAlgorithm(WeightsGrid weightsGrid, int generationSize, 
            ISelector selector, 
            IMutator<int> mutator, double mutationProbability, 
            ICrosser<int> crosser, double crossoverProbability)
        {
            _weightsGrid = weightsGrid;
            _generation = GetRandomGeneration(generationSize, weightsGrid);
            Previous = Individual.GetBest(_generation);
            Best = Previous;
            BestForIteration = Previous;
            Iteration = 0;
            _selector = selector;
            _mutator = mutator;
            _mutationProbability = mutationProbability;
            _crosser = crosser;
            _crossoverProbability = crossoverProbability;
        }
        
        public static List<Individual> GetRandomGeneration(int generationSize, WeightsGrid weightsGrid)
        {
            var generation = new List<Individual>();
            for (int i = generationSize - 1; i >= 0; i--)
            {
                generation.Add(Individual.GetRandom(weightsGrid));
            }

            return generation;
        } 

        public void RunIteration()
        {
            SetNewGeneration();
            Previous = Best;
            BestForIteration = Individual.GetBest(_generation);
            Iteration++;
            
            if (Best.Score > BestForIteration.Score)
            {
                Best = BestForIteration;
                LastIterationWithNewBestDiscovered = Iteration;
                BetweenChangesDiffPrev = BetweenChangesDiff;
                BetweenChangesDiff = 0;
            }
            else
            {
                BetweenChangesDiff++;
            }
        }

        public int GetIterationNumber()
        {
            return Iteration;
        }

        public double GetBestScore()
        {
            return Best.Score;
        }

        public double GetBestForIterationScore()
        {
            return BestForIteration.Score;
        }

        public List<int> GetBestGenotype()
        {
            return Best.Genotype;
        }

        public bool WasNewBestDiscovered()
        {
            return LastIterationWithNewBestDiscovered == Iteration;
        }

        public double GetDecreasePercentage()
        {
            return (1 - (Best.Score / Previous.Score)) * 100;
        }

        public int NumbOfIterationsSincePrevChange()
        {
            if (WasNewBestDiscovered())
            {
                return BetweenChangesDiffPrev;
            }

            return BetweenChangesDiff;
        }

        private void SetNewGeneration()
        {
            var nextGeneration = new List<Individual>();
            while (nextGeneration.Count < _generation.Count)
            {
                var p1 = _selector.Get(_generation);
                var p2 = _selector.Get(_generation);

                List<int> childGenotype = p1.Genotype;
                if (_crossoverProbability < Random.NextDouble())
                {
                    childGenotype = _crosser.Get(p1.Genotype, p2.Genotype);
                }

                if (_mutationProbability < Random.NextDouble())
                {
                    childGenotype = _mutator.Get(childGenotype);
                }

                var child = new Individual(childGenotype, _weightsGrid);
                nextGeneration.Add(child);
            }

            _generation = nextGeneration;
        }
    }
}
