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
                var bestResult = 2020;
                var generationSize = 100;
                
                var grid = new WeightsGrid(testcases[j].Weights);
                var selector = new SelectorTournament();
                selector.SetArgs(0.3);
                var ga = new GeneticAlgorithm(grid, generationSize, 
                    selector,
                    new MutatorReverseSequence<int>(), 0.3,
                    new CrosserPartiallyMatched(), 0.3
                    );
        
                var prevBest = ga.Best.Score;
                for (int i = 0; i < 100 || bestResult >= ga.Best.Score; i++)
                {
                    Assert.AreEqual(i, ga.Iteration);
                    ga.RunIteration();
                    Assert.LessOrEqual(ga.Best.Score, prevBest);
                    prevBest = ga.Best.Score;
                    Assert.LessOrEqual(ga.Best.Score, ga.BestForIteration.Score);
                    Assert.LessOrEqual(ga.LastIterationWithNewBestDiscovered, ga.Iteration);
                }
                Assert.LessOrEqual(bestResult, ga.Best.Score);
            };
        }
    }
}