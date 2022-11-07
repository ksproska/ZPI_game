using System.Collections.Generic;
using NUnit.Framework;

namespace Maps
{
    [TestFixture]
    public class MapUtilsTests
    {
        class DistanceMatrixTestCase
        {
            public readonly Map map;
            public readonly List<List<double>> distMatrix;
            public DistanceMatrixTestCase(Map map, List<List<double>> distMatrix)
            {
                this.map = map;
                this.distMatrix = distMatrix;
            }
        }
        class DistanceTestCase
        {
            public readonly Point pointA;
            public readonly Point pointB;
            public readonly double dist;
            public DistanceTestCase(Point pointA, Point pointB, double distance)
            {
                this.pointA = pointA;
                this.pointB = pointB;
                this.dist = distance;
            }
        }
        [Test]
        public static void GetDistanceMatrixTest()
        {
            List<DistanceMatrixTestCase> testCases = new List<DistanceMatrixTestCase>() 
            { 
                new DistanceMatrixTestCase(new Map(new List<Point>() {new Point(245, 111), new Point(95, 1063)}), 
                    new List<List<double>> {new List<double>() { 0, 963.74 }, new List<double>() { 963.74, 0 } }),
                new DistanceMatrixTestCase(new Map(new List<Point>()), new List<List<double>>()),
                new DistanceMatrixTestCase(new Map(new List<Point>() {new Point(652, 93), new Point(245, 789), new Point(331, 475) }),
                    new List<List<double>> {new List<double>() { 0, 806.27, 498.96}, new List<double>() { 806.27, 0, 325.56 }, new List<double>() { 498.96, 325.56, 0 } }),
                new DistanceMatrixTestCase(new Map(new List<Point>() {new Point(684, 722), new Point(635, 20), new Point(701, 107) }),
                    new List<List<double>> {new List<double>() { 0, 703.71, 615.23}, new List<double>() { 703.71, 0, 109.20 }, new List<double>() { 615.23, 109.20, 0 } }),
                new DistanceMatrixTestCase(new Map(new List<Point>() {new Point(122, 983), new Point(582, 907), new Point(341, 123) }),
                    new List<List<double>> {new List<double>() { 0, 466.24, 887.45}, new List<double>() { 466.24, 0, 820.21 }, new List<double>() { 887.45, 820.21, 0 } })
            };
            foreach(var testCase in testCases)
            {
                List<List<double>> actual = MapUtils.GetDistanceMatrix(testCase.map);
                List<List<double>> expected = testCase.distMatrix;
                Assert.AreEqual(expected.Count, actual.Count);
                for(int rowInx = 0; rowInx < actual.Count; rowInx++)
                {
                    Assert.AreEqual(expected[rowInx].Count, actual[rowInx].Count);
                    for(int colInx = 0; colInx < actual[rowInx].Count; colInx++)
                    {
                        Assert.AreEqual(expected[rowInx][colInx], actual[rowInx][colInx], 0.01);
                    }
                }
            }
        }
    }

}
