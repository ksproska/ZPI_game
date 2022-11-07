using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GA 
{ 
    [TestFixture]
    public class CrosserCycleTests
    {
        class TestCase
        {
            public readonly List<int> Parent1, Parent2, Child;
            public readonly List<List<int>> Cycles;

            public TestCase(List<int> parent1, List<int> parent2, List<List<int>> cycles, List<int> child)
            {
                this.Parent1 = parent1;
                this.Parent2 = parent2;
                this.Cycles = cycles;
                this.Child = child;
            }
        }
        private static readonly List<TestCase> TestCases = new ()
        {
            new (
                new List<int>{8, 4, 7, 3, 6, 2, 5, 1, 9, 0},
                new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                new List<List<int>>
                {
                    new() {0, 9, 8},
                    new () {1, 7, 2, 5, 6, 4},
                    new () {3},
                },
                new List<int>{8, 1, 2, 3, 4, 5, 6, 7, 9, 0}
                ),
                
        };
        [Test]
        public static void TestCross()
        {
            foreach (var testCase in TestCases)
            {
                var actual = CrosserCycle.Cross(testCase.Parent1, testCase.Parent2);
                Assert.IsTrue(actual.SequenceEqual(testCase.Child));
            }
            
        }
        [Test]
        public static void TestGetCycles()
        {
            foreach (var testCase in TestCases)
            {
                var actual = CrosserCycle.GetCycles(testCase.Parent1, testCase.Parent2);
                Assert.AreEqual(actual.Count, testCase.Cycles.Count);
                for (int i = 0; i < testCase.Cycles.Count; i++)
                {
                    Assert.IsTrue(actual[i].SequenceEqual(testCase.Cycles[i]));
                }
            }
        }
    }
}