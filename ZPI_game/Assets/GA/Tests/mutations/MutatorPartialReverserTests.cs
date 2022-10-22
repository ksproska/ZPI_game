using System.Collections.Generic;
using System.Linq;
using GA.mutations;
using NUnit.Framework;

namespace GA
{
    [TestFixture]
    public class MutatorPartialReverserTests
    {
        class TestCase
        {
            // Nazewnictwo zmiennych niezgodne z konwencją
            public readonly List<int> Genotype, Expected;
            public readonly int StartInx, EndInx;

            public TestCase(List<int> genotype, List<int> expected, int startInx, int endInx)
            {
                this.Genotype = genotype;
                this.Expected = expected;
                this.StartInx = startInx;
                this.EndInx = endInx;
            }
        }
        [Test]
        public static void TestMutate()
        {
            var testCases = new List<TestCase>()
            {
                new TestCase(
                    new List<int>() {0, 1, 2, 3, 4, 5},
                    new List<int>() {5, 4, 3, 2, 1, 0},
                    0, 5),
                new TestCase(
                    new List<int>() {0, 1, 2, 3, 4, 5},
                    new List<int>() {0, 4, 3, 2, 1, 5},
                    1, 4),
                new TestCase(
                    new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                    new List<int>() {0, 1, 7, 6, 5, 4, 3, 2, 8, 9},
                    2, 7),
                new TestCase(
                    new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                    new List<int>() {9, 8, 7, 3, 4, 5, 6, 2, 1, 0},
                    7, 2),
            };
            foreach (var testCase in testCases)
            {
                var actual = MutatorPartialReverser<int>.ReversePartOrder(testCase.Genotype, testCase.StartInx, testCase.EndInx);
                Assert.IsTrue(actual.SequenceEqual(testCase.Expected));
            }
        }
    }
}