using Assets.GA.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GA.Tests
{
    [TestFixture]
    class RecordedListTests
    {
        [Test]
        public static void AddElementsTest()
        {
            RecordedList<int> list = new RecordedList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(4, list.History.Count);
            Assert.AreEqual(0, list.InitState.Count);
            CollectionAssert.AreEqual(list.InitState, new List<int>());
            CollectionAssert.AreEqual(new List<int>(new int[] { 1 }), list.History[1]);
            CollectionAssert.AreEqual(new List<int>(new int[] { 1, 2 }), list.History[2]);
            CollectionAssert.AreEqual(new List<int>(new int[] { 1, 2, 3 }), list.History[3]);
        }

        [Test]
        public static void AddRangeTest()
        {
            RecordedList<int> list = new RecordedList<int>(new int[] { 1, 2, 3, 4 });
            List<int> listToAdd = new List<int>(new int[] { 1, 2, 3 });
            list.AddRange(listToAdd);

            Assert.AreEqual(2, list.History.Count);
            CollectionAssert.AreEqual(new List<int>(new int[] { 1, 2, 3, 4, 1, 2, 3 }), list.CurrentState);
            CollectionAssert.AreEqual(new List<int>(new int[] { 1, 2, 3, 4 }), list.InitState);
            Assert.AreEqual(4, list.InitState.Count);
            Assert.AreEqual(7, list.CurrentState.Count);
        }

        [Test]
        public static void GetHistoryDifferenceTest()
        {
            RecordedList<int> list = new RecordedList<int>(new int[] { 1, 2, 3, 4 });
            list[0] = 7;
            list[0] = 2;
            list.Add(5);
            list.AddRange(new List<int>(new int[] { 6, 7 }));
            List<(int, int)> expected = new List<(int, int)>(new (int, int)[]
            {
                (0, 7), (0, 2), (4, 5), (5, 6), (6, 7)
            });
            CollectionAssert.AreEqual(expected, list.GetHistoryDifference());
        }
    }
}
