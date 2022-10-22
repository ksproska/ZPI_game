using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GA
{
    [TestFixture]
    public class CrosserPartiallyMatchedTests
    {
        class TestCase
        {
            public readonly List<int> Parent1, Parent2, Child;
            public readonly int StartInx, SegmentLen;

            public TestCase(List<int> parent1, List<int> parent2, List<int> child, int startInx, int segmentLen)
            {
                this.Parent1 = parent1;
                this.Parent2 = parent2;
                this.Child = child;
                this.StartInx = startInx;
                this.SegmentLen = segmentLen;
            }
        }
        private static readonly List<TestCase> TestCases = new List<TestCase>()
        {
            new TestCase(
                new List<int>(){8, 4, 7, 3, 6, 2, 5, 1, 9, 0},
                new List<int>(){0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                new List<int>(){0, 7, 4, 3, 6, 2, 5, 1, 8, 9},
                3, 5),
            new TestCase(
                new List<int>(){1, 2, 3, 4, 5, 6, 7, 8},
                new List<int>(){4, 3, 2, 7, 6, 5, 8, 1},
                new List<int>(){7, 3, 2, 4, 5, 6, 8, 1},
                3, 2),
            new TestCase(
                new List<int>(){1, 2, 3, 5, 4, 6},
                new List<int>(){4, 3, 2, 6, 1, 5},
                new List<int>(){4, 2, 3, 6, 1, 5},
                1, 2),
                /** można jeszcze taki przypadek
            new TestCase(
                new List<int>(){7, 9, 2, 3, 5, 1, 8, 6, 4},
                new List<int>(){7, 9, 2, 3, 5, 1, 8, 6, 4},
                new List<int>(){7, 9, 2, 3, 5, 1, 8, 6, 4},
                3, 5) **/
                /** ewentualnie jeszcze taki
                ,
            new TestCase(
                new List<int>(){1, 4, 2, 8, 5, 7, 3, 6, 9},
                new List<int>(){7, 5, 3, 1, 9, 8, 6, 4, 2},
                new List<int>(){7, 5, 3, 1, 9, 8, 6, 4, 2},
                4, 0) **/
                
        };
        [Test]
        public static void TestCross()
        {
            foreach (var testCase in TestCases)
            {
                var actual = CrosserPartiallyMatched.Cross(testCase.Parent1, testCase.Parent2, testCase.StartInx, testCase.SegmentLen); 
                Assert.IsTrue(actual.SequenceEqual(testCase.Child));
            }
            
        }
    }
}