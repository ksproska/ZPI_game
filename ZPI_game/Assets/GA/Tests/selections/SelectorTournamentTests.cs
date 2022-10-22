using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GA
{
    [TestFixture]
    public class SelectorTournamentTests
    {
        [Test]
        public static void TestGetRandom()
        {
            var max = 10;
            var len = 3;
            var selected = SelectionCommonMethods.GetRandom(max, len);
            Assert.AreEqual(len, selected.Count);
            
            var ordered = selected.OrderBy(o=>o).ToList();
            Assert.Less(ordered[ordered.Count - 1], max);
            Assert.LessOrEqual(0, ordered[0]);
        }
        
        [Test]
        public static void TestGetForIndexes()
        {
            var testcases = TravellingSalesmanTestCase.Get();
            for (int i = testcases.Count - 1; i >= 0; i--)
            {
                var grid = new WeightsGrid(testcases[i].Weights);
                var generationSize = 10;
                var numbOfSelected = 3;
                var initGeneration = GeneticAlgorithm.GetRandomGeneration(generationSize, grid);
                var selectedIndexes = SelectionCommonMethods.GetRandom(generationSize, numbOfSelected);
                
                var selected = SelectionCommonMethods.GetForIndexes(initGeneration, selectedIndexes);
                Assert.AreEqual(numbOfSelected, selected.Count);
                foreach (var inx in selectedIndexes)
                {
                    Assert.Contains(initGeneration[inx], selected);
                }
            };
        }

        class TestCase
        {
            // nazewnictwo zmiennych niezgodne z konwencją
            public readonly int GenerationSize, TournamentSize;

            public TestCase(int generationSize, int tournamentSize)
            {
                GenerationSize = generationSize;
                TournamentSize = tournamentSize;
            }
        }

        [Test]
        public static void GetTournament()
        {
            var testCases = new List<TestCase>()
            {
                new TestCase(10, 3),
                new TestCase(10, 1),
                new TestCase(10, 10)
            };
            foreach (var testCase in testCases)
            {
                var testcases = TravellingSalesmanTestCase.Get();
                for (int i = testcases.Count - 1; i >= 0; i--)
                {
                    var grid = new WeightsGrid(testcases[i].Weights);
                    var initGeneration = GeneticAlgorithm.GetRandomGeneration(testCase.GenerationSize, grid);
                    var winner = SelectorTournament.GetTournament(initGeneration, testCase.TournamentSize);
        
                    var generationSorted = initGeneration.OrderBy(o => o.Score).ToList();
                    var generationSortedTrimmed = generationSorted.Select(o => o.Score).Take(
                        (testCase.GenerationSize - (testCase.TournamentSize - 1))
                        ).ToList();
                    Assert.Contains(winner.Score, generationSortedTrimmed);
                };
            }
        }
    }
}
