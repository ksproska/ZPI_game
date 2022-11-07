using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace GA
{
    [TestFixture]
    public class GeneticAlgorithmProxyTests
    {
        [Test]
        public static void TestGet()
        {
            var testcases = TravellingSalesmanTestCase.Get();
            for (int j = testcases.Count - 1; j >= 0; j--)
            {
                var ga = GeneticAlgorithmProxy.Get(testcases[j].Weights, 30,
                    MutationType.RSM, 0.3,
                    CrossoverType.PartiallyMatched, 0.3,
                    SelectionType.Tournament, 0.3
                );
                var prettifier = GeneticAlgorithmProxy.GetPrettifier(ga);
                var generationSize = 100;
                
                var prevBest = ga.GetBestScore();
                for (int i = 0; i < 100 || testcases[j].BestScore >= ga.GetBestScore(); i++)
                {
                    Assert.AreEqual(i, ga.GetIterationNumber());
                    ga.RunIteration();
                    Console.Write(prettifier.GetCurrentIterationLogIfNewBestFound());
                    Assert.LessOrEqual(ga.GetBestScore(), prevBest);
                    prevBest = ga.GetBestScore();
                    Assert.LessOrEqual(ga.GetBestScore(), ga.GetBestForIterationScore());
                }
                Assert.LessOrEqual(testcases[j].BestScore, ga.GetBestScore());
            }
        }
    }
}
