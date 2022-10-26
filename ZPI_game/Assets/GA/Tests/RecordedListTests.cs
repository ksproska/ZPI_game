using Assets.GA.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Tests
{
    [TestFixture]
    class RecordedListTests
    {
        [Test]
        public static void GetElementTest()
        {
            var list = new RecordedList<int>(new[] { 1, 2, 3, 4 });
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
            Assert.AreEqual(4, list[3]);
            Assert.Catch(typeof(ArgumentOutOfRangeException), () =>
            {
                var _ = list[5];
            });
        }
        
        [Test]
        public static void AddElementsTest()
        {
            RecordedList<int> list = new RecordedList<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(3, list.History.Count);
            Assert.AreEqual((0, 1), list.History[0]);
            Assert.AreEqual((1, 2), list.History[1]);
            Assert.AreEqual((2, 3), list.History[2]);
            CollectionAssert.AreEqual(new List<int>(new [] { 1, 2, 3 }), list.CurrentState);
        }

        [Test]
        public static void AddRangeTest()
        {
            RecordedList<int> list = new RecordedList<int>(new int[] { 1, 2, 3, 4 });
            List<int> listToAdd = new List<int>(new int[] { 1, 2, 3 });
            list.AddRange(listToAdd);

            Assert.AreEqual(3, list.History.Count);
            CollectionAssert.AreEqual(new List<int>(new int[] { 1, 2, 3, 4, 1, 2, 3 }), list.CurrentState);
            Assert.AreEqual(7, list.CurrentState.Count);
        }

        [Test]
        public static void GetHistoryDifferenceTest()
        {
            RecordedList<int> list = new RecordedList<int>(new int[] { 1, 2, 3, 4 });
            list[0] = 1;
            list[0] = 2;
            list.Add(5);
            list.AddRange(new List<int>(new int[] { 6, 7 }));
            List<(int, int)> expected = new List<(int, int)>(new (int, int)[]
            {
                (0, 1), (0, 2), (4, 5), (5, 6), (6, 7)
            });
            CollectionAssert.AreEqual(expected, list.History);
        }

        [Test]
        public static void SwapElementsWithTuplesTest()
        {
            RecordedList<int> list = new RecordedList<int>(new int[] { 1, 2, 3 });
            (list[0], list[1]) = (list[1], list[0]);
            Assert.AreEqual(2, list.History.Count);
            CollectionAssert.AreEqual(new (int, int)[] {(0, 2), (1, 1) }, list.History);
        }

        [Test]
        public static void GetFullHistoryTest()
        {
            RecordedList<int> list = new RecordedList<int>(new int[] { 1, 2, 3, 4, 5});
            list.Add(6);
            list[0] = 10;
            list[2] = 3;
            CollectionAssert.AreEqual(new List<(int, int)>(new (int, int)[]
            {
                (5, 6), (0, 10), (2, 3), (1, 2), (3, 4), (4, 5)
            }), list.GetFullHistory());

            list[0] = 1;
            CollectionAssert.AreEqual(new List<(int, int)>(new (int, int)[]
            {
                (5, 6), (0, 10), (2, 3), (0, 1), (1, 2), (3, 4), (4, 5)
            }), list.GetFullHistory());
        }

        [Test]
        public static void ElementSetToThSameValueTest()
        {
            var list = new RecordedList<int>(new[] { 1, 2, 3, 4 });
            list[0] = 1;
            list[0] = 1;
            list[1] = 2;
            list[0] = 1;
            CollectionAssert.AreEqual(new [] {(0, 1), (0, 1), (1, 2), (0, 1)}, list.History);
            CollectionAssert.AreEqual(new []
            {
                (0, 1), (0, 1), (1, 2), (0, 1), (2, 3), (3, 4)
            }, list.GetFullHistory());
        }
    }
}
