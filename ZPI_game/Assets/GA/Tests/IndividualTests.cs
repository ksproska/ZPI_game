using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GA
{
    [TestFixture]
    public class IndividualTests
    {
        [Test]
        public static void TestCountScore() {
            var testcases = TravellingSalesmanTestCase.Get();
            for (int i = 0; i < testcases.Count; i++)
            {
                var grid = new WeightsGrid(testcases[i].Weights);
                var actual = Individual.GetScore(testcases[i].BestGenotype, grid);
                Assert.AreEqual(testcases[i].BestScore, actual);
            };
        }
        
        [Test]
        public static void TestMember()
        {
            var testcases = TravellingSalesmanTestCase.Get();
            for (int i = 0; i < testcases.Count; i++)
            {
                var grid = new WeightsGrid(testcases[i].Weights);
                var actual = new Individual(testcases[i].BestGenotype, grid);
                Assert.AreEqual(testcases[i].BestScore, actual.Score);
            };
        }
        
        [Test]
        public static void TestGetRandom()
        {
            var testcases = TravellingSalesmanTestCase.Get();
            for (int i = 0; i < testcases.Count; i++)
            {
                var grid = new WeightsGrid(testcases[i].Weights);
                var member = Individual.GetRandom(grid);
                Assert.AreEqual(grid.Size, member.Genotype.Count);
                var ordered = member.Genotype.OrderBy(o=>o).ToList();
                Assert.AreEqual(0, ordered[0]);
                Assert.AreEqual(grid.Size - 1, ordered[ordered.Count - 1]);
                var set = member.Genotype.ToHashSet();
                Assert.AreEqual(grid.Size, set.Count);
            };
        }
        
        [Test]
        public static void TestGetBestScore()
        {
            var members = new List<Individual>(){new Individual(5), new Individual(1), new Individual(7)};
            Assert.AreEqual(1, Individual.GetBest(members).Score);
        }
    }
}
