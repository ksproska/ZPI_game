using System;
using System.Collections.Generic;
using NUnit.Framework;


namespace GA
{
    [TestFixture]
    public class WeightsGridTests
    {
        [Test]
        public void TestLoad_correct()
        {
            var testcases = TravellingSalesmanTestCase.Get();
            for (int i = 0; i < testcases.Count; i++)
            {
                var grid = new WeightsGrid(testcases[i].Weights);
                Assert.AreEqual(testcases[i].Weights.Count, grid.Size);
            };
        }
        [Test]
        public void TestLoad_incorrect()
        {
            var testcases = new List<List<List<double>>>()
            {
                new List<List<double>>()
                {
                    new List<double>(){1, 1},
                    new List<double>(){1, 1},
                },
                new List<List<double>>()
                {
                    new List<double>(){0, 1},
                    new List<double>(){2, 0},
                },
                new List<List<double>>()
                {
                    new List<double>(){0, 1},
                    new List<double>(){2},
                },
                new List<List<double>>()
                {
                    new List<double>(){0, 1, 2},
                    new List<double>(){1, -1, 3},
                    new List<double>(){2, 3, 0},
                },
            };
            // Nie lepiej AssertThrows?
            /**
                Assert.Throws<InvalidOperationException>(() => new WeightsGrid(testcases[i]))
            **/
            for (int i = testcases.Count - 1; i >= 0; i--)
            {
                try {
                    new WeightsGrid(testcases[i]);
                } 
                catch (InvalidOperationException) {}
                catch (Exception) {
                    Assert.Fail();
                }
            };
        }
    }
}
