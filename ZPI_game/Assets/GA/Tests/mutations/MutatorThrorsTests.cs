using System;
using System.Collections.Generic;
using System.Linq;
using GA.mutations;
using NUnit.Framework;

namespace GA
{
    [TestFixture]
    public class MutatorThrorsTests
    {
        class TestCase
        {
            // Nazewnictwo zmiennych niezgodne z konwencją
            public readonly List<int> Genotype, Expected;
            public readonly List<int> Indexes;

            public TestCase(List<int> genotype, List<int> expected, List<int> indexes)
            {
                this.Genotype = genotype;
                this.Expected = expected;
                this.Indexes = indexes;
            }
        }
        [Test]
        public static void TestMutate()
        {
            var testCases = new List<TestCase>()
            {
                new TestCase(
                    new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                    new List<int>() {4, 1, 0, 3, 2, 5, 6, 7, 8, 9},
                    new List<int>() {0, 2, 4}
                    ),
            };
            foreach (var testCase in testCases)
            {
                var actual = MutatorThrors<int>.SwipeOneRight(testCase.Genotype, testCase.Indexes);
                Assert.IsTrue(actual.SequenceEqual(testCase.Expected));
            }
        }
    }
}