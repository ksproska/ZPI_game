using System.Collections.Generic;
using NUnit.Framework;

namespace GA
{
    [TestFixture]
    public class SelectorRouletteTests
    {
        class TestCase
        {
            // nazewnictwo zmiennych niezgodne z konwencją
            public readonly double[] ActualInput;
            public readonly List<double> Expected;
            public readonly double Value;
            public readonly int Index;

            public TestCase(double[] actualInput, List<double> expected, double value, int index)
            {
                ActualInput = actualInput;
                Expected = expected;
                Value = value;
                Index = index;
            }
        }

        private static readonly List<TestCase> testCases = new List<TestCase>()
        {
            new TestCase(new double[] {1, 4, 2, 3}, new List<double> {0.4, 0.5, 0.8, 1}, 0.6, 2),
            new TestCase(new double[] {3, 2, 3, 1, 2, 3}, new List<double> {0.1, 0.3, 0.4, 0.7, 1}, 0.9, 4),
            new TestCase(new double[] {2, 1, 2, 1, 2, 1, 2}, new List<double> {0.1, 0.3, 0.4, 0.6, 0.7, 0.9, 1}, 0, 0),
            /**
            można jeszcze 
            new TestCase(new double[] {1, 1, 1, 1, 1}, new List<double> {0.2, 0.4, 0.6, 0.8, 1}, 0.25, 1),
            **/
        };
        
        [Test]
        public static void TestGetDistributedWeights()
        {
            foreach (var testCase in testCases)
            {
                var generation = new List<Individual>{};
                foreach(var input in testCase.ActualInput) {
                    generation.Add(new Individual(input));
                }

                var actual = SelectorRoulette.GetDistributedWeights(generation);
                for(int i = 0; i < testCase.Expected.Count; i++) {
                    Assert.AreEqual(testCase.Expected[i], actual[i], 0.11);
                }
            }
        }

        [Test]
        public static void TestGetIndexForValue()
        {
            foreach (var testCase in testCases)
            {
                var actualIndex = SelectorRoulette.GetIndexForValue(testCase.Expected, testCase.Value);
                Assert.AreEqual(testCase.Index, actualIndex);
            }
        }
    }
}