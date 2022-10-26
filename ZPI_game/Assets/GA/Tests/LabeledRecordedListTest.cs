using System.Collections.Generic;
using Assets.GA.Utils;
using NUnit.Framework;

namespace GA.Tests
{
    [TestFixture]
    public class LabeledRecordedListTest
    {
        [Test]
        public static void AddElementsTest()
        {
            var list = new LabeledRecordedList<string, int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(3, list.History.Count);
            
            Assert.AreEqual(0, list.History[0].Item1);
            Assert.AreEqual(1, list.History[0].Item2);
            Assert.AreEqual(null, list.History[0].Item3);
            
            Assert.AreEqual(1, list.History[1].Item1);
            Assert.AreEqual(2, list.History[1].Item2);
            Assert.AreEqual(null, list.History[1].Item3);
            
            Assert.AreEqual(2, list.History[2].Item1);
            Assert.AreEqual(3, list.History[2].Item2);
            Assert.AreEqual(null, list.History[2].Item3);
            
            CollectionAssert.AreEqual(new List<int>(new [] { 1, 2, 3 }), list.CurrentState);
        }

        [Test]
        public static void AddLabeledTest()
        {
            var list = new LabeledRecordedList<string, int>();
            list.AddLabeled(1, "one");
            list.AddLabeled(2, "two");
            list.AddLabeled(3, "three");
            Assert.AreEqual(3, list.History.Count);
            
            Assert.AreEqual(0, list.History[0].Item1);
            Assert.AreEqual(1, list.History[0].Item2);
            Assert.AreEqual("one", list.History[0].Item3);
            
            Assert.AreEqual(1, list.History[1].Item1);
            Assert.AreEqual(2, list.History[1].Item2);
            Assert.AreEqual("two", list.History[1].Item3);
            
            Assert.AreEqual(2, list.History[2].Item1);
            Assert.AreEqual(3, list.History[2].Item2);
            Assert.AreEqual("three", list.History[2].Item3);
            
            CollectionAssert.AreEqual(new List<int>(new [] { 1, 2, 3 }), list.CurrentState);
        }
        
        [Test]
        public static void AddRangeTest()
        {
            var list = new LabeledRecordedList<string, int>(new [] { 1, 2, 3, 4 });
            List<int> listToAdd = new List<int>(new int[] { 1, 2, 3 });
            list.AddRange(listToAdd);

            Assert.AreEqual(3, list.History.Count);
            CollectionAssert.AreEqual(new List<int>(new [] { 1, 2, 3, 4, 1, 2, 3 }), list.CurrentState);
            Assert.AreEqual(7, list.CurrentState.Count);
        }

        [Test]
        public static void AddLabeledRangeTest()
        {
            var list = new LabeledRecordedList<string, int>(new [] { 1, 2, 3, 4 });
            var listToAdd = new List<(int, string)>(new [] { (1, "one"), (2, "two"), (3, "three") });
            list.AddRangeLabeled(listToAdd);
            
            Assert.AreEqual(3, list.History.Count);
            CollectionAssert.AreEqual(new List<int>(new [] { 1, 2, 3, 4, 1, 2, 3 }), list.CurrentState);
            Assert.AreEqual(7, list.CurrentState.Count);
            
            Assert.AreEqual(4, list.History[0].Item1);
            Assert.AreEqual(1, list.History[0].Item2);
            Assert.AreEqual("one", list.History[0].Item3);
            
            Assert.AreEqual(5, list.History[1].Item1);
            Assert.AreEqual(2, list.History[1].Item2);
            Assert.AreEqual("two", list.History[1].Item3);
            
            Assert.AreEqual(6, list.History[2].Item1);
            Assert.AreEqual(3, list.History[2].Item2);
            Assert.AreEqual("three", list.History[2].Item3);
        }
        
        [Test]
        public static void GetHistoryDifferenceTest()
        {
            var list = new LabeledRecordedList<string, int>(new [] { 1, 2, 3, 4 });
            list[0] = 1;
            list.SetLabeled(0, 2, "label");
            list.Add(5);
            list.AddLabeled(5, "label");
            list.AddRange(new List<int>(new [] { 6, 7 }));
            List<(int, int, string)> expected = new List<(int, int, string)>(new []
            {
                (0, 1, null), (0, 2, "label"), (4, 5, null), (5, 5, "label"), (6, 6, null), (7, 7, null)
            });
            CollectionAssert.AreEqual(expected, list.History);
        }
        
        [Test]
        public static void GetFullHistoryTest()
        {
            var list = new LabeledRecordedList<string, int>(new [] { 1, 2, 3, 4, 5 });
            list.Add(6);
            list[0] = 10;
            list.SetLabeled(2, 3, "label");
            CollectionAssert.AreEqual(new List<(int, int, string)>(new []
            {
                (5, 6, null), (0, 10, null), (2, 3, "label"), (1, 2, null), (3, 4, null), (4, 5, null)
            }), list.GetFullHistory());

            list.SetLabeled(0, 1, "label");
            CollectionAssert.AreEqual(new List<(int, int, string)>(new []
            {
                (5, 6, null), (0, 10, null), (2, 3, "label"), (0, 1, "label"), (1, 2, null), (3, 4, null), (4, 5, null)
            }), list.GetFullHistory());
        }
    }
}