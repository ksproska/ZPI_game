using System.Collections.Generic;
using GA.mutations;
using NUnit.Framework;

namespace GA
{
    [TestFixture]
    public class GeneticAlgorithmTests
    {
        [Test]
        public static void TestGetRandomGeneration()
        {
            var testcases = TravellingSalesmanTestCase.Get();
            for (int i = testcases.Count - 1; i >= 0; i--)
            {
                var grid = new WeightsGrid(testcases[i].Weights);
                var expectedSize = 10;
                var initGeneration = GeneticAlgorithm.GetRandomGeneration(expectedSize, grid);
                Assert.AreEqual(expectedSize, initGeneration.Count);
            };
        }
        
        [Test]
        public static void TestRunIteration()
        {
            var testcases = TravellingSalesmanTestCase.Get();
            for (int j = testcases.Count - 1; j >= 0; j--)
            {
                var bestResult = testcases[j].BestScore;
                var generationSize = 100;
                var numbOfIterations = 100;
                var mutationProbability = 0.3;
                var crossoverProbability = 0.3;
                
                var grid = new WeightsGrid(testcases[j].Weights);
                var selectorTournament = new SelectorTournament();
                selectorTournament.SetArgs(0.3);
                
                foreach (var selector in new List<ISelector>(){selectorTournament, new SelectorRoulette()})
                {
                    foreach (var crosser in new List<ICrosser<int>>(){new CrosserPartiallyMatched(), new CrosserCycle(), new CrosserOrder()})
                    {
                        foreach (var mutator in new List<IMutator<int>>(){new MutatorReverseSequence<int>(), new MutatorThrors<int>()})
                        {
                            var ga = new GeneticAlgorithm(grid, generationSize, 
                                 selector,
                                 mutator, mutationProbability,
                                 crosser, crossoverProbability
                                 );
                             var prevBest = ga.Best.Score;
                             for (int i = 0; i < numbOfIterations || bestResult >= ga.Best.Score; i++)
                             {
                                 Assert.AreEqual(i, ga.Iteration);
                                 ga.RunIteration();
                                 Assert.LessOrEqual(ga.Best.Score, prevBest);
                                 prevBest = ga.Best.Score;
                                 Assert.LessOrEqual(ga.Best.Score, ga.BestForIteration.Score);
                                 Assert.LessOrEqual(ga.LastIterationWithNewBestDiscovered, ga.Iteration);
                             }
                             Assert.LessOrEqual(bestResult, ga.Best.Score);
                        }
                    }
                }
            };
        }
    }
}