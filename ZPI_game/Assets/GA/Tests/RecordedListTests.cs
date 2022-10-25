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
            Assert.Equals(list.History.Count, 4);
            CollectionAssert.AreEqual(list.InitState, new List<int>(new int[] { }));
            CollectionAssert.AreEqual(list.History[1], new List<int>(new int[] { 1 }));
            CollectionAssert.AreEqual(list.History[2], new List<int>(new int[] { 1, 2 }));
            CollectionAssert.AreEqual(list.History[3], new List<int>(new int[] { 1, 2, 3 }));
            CollectionAssert.AreEqual(list.History[4], new List<int>(new int[] { 1, 2, 3, 4 }));
        }
    }
}
