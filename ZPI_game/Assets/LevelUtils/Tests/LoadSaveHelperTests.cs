using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace LevelUtils
{
    [TestFixture]
    public class LoadSaveHelperTests
    {
        class TestCase
        {
            public readonly List<int> savedLevels;

            public TestCase(List<int> savedLevels)
            {
                this.savedLevels = savedLevels;
            }
        }
        [SetUp]
        public static void Init()
        {
            LoadSaveHelper.EraseAllSlots();
        }
        [Test]
        public static void TestCompleteALevel()
        {
            LoadSaveHelper.EraseAllSlots();
            
            List<TestCase> testCases = new List<TestCase>()
            {
                new TestCase(new List<int>() {1, 2}),
                new TestCase(new List<int>() {1, 2, 3, 4, 5, 7, 8}),
                new TestCase(new List<int>() {1, 2, 3})
            };

            LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.First);
            LoadSaveHelper.CompleteALevel(2, LoadSaveHelper.SlotNum.First);
            

            LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.CompleteALevel(2, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.CompleteALevel(3, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.CompleteALevel(4, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.CompleteALevel(5, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.CompleteALevel(7, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.CompleteALevel(8, LoadSaveHelper.SlotNum.Second);

            LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.Third);
            LoadSaveHelper.CompleteALevel(2, LoadSaveHelper.SlotNum.Third);
            LoadSaveHelper.CompleteALevel(3, LoadSaveHelper.SlotNum.Third);
            Assert.Throws<ArgumentException>(() => LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.First));
            var jsonStructuredDict = new Dictionary<string, List<Dictionary<string, List<int>>>>();
            var listOfSlots = new List<Dictionary<string, List<int>>>();
            foreach (List<int> levels in testCases.Select(el => el.savedLevels))
            {
                var jsonDict = new Dictionary<string, List<int>>();
                jsonDict.Add("levels_completed", levels);
                listOfSlots.Add(jsonDict);
            }
            jsonStructuredDict.Add("slots", listOfSlots);

            string jsonFile = File.ReadAllText(LoadSaveHelper.JSON_FILE_NAME);
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, List<int>>>>>(jsonFile);
            CollectionAssert.AreEquivalent(jsonStructuredDict, parsedJson);
            LoadSaveHelper.EraseAllSlots();

        }

        [Test]
        public static void TestGetSlot()
        {

            List<TestCase> testCases = new List<TestCase>()
            {
                new TestCase(new List<int>() {}),
                new TestCase(new List<int>() {}),
                new TestCase(new List<int>() {})
            };
            List<int> firstSlot = LoadSaveHelper.GetSlot(LoadSaveHelper.SlotNum.First);
            List<int> secondSlot = LoadSaveHelper.GetSlot(LoadSaveHelper.SlotNum.Second);
            List<int> thirdSlot = LoadSaveHelper.GetSlot(LoadSaveHelper.SlotNum.Third);

            CollectionAssert.AreEqual(testCases[0].savedLevels, firstSlot);
            CollectionAssert.AreEqual(testCases[1].savedLevels, secondSlot);
            CollectionAssert.AreEqual(testCases[2].savedLevels, thirdSlot);
            
        }

        [Test]
        public static void GetOccupiedSlotsTest()
        {
            Assert.AreEqual(0, LoadSaveHelper.GetOccupiedSlots().Count);

            LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.First);
            LoadSaveHelper.CompleteALevel(2, LoadSaveHelper.SlotNum.First);

            CollectionAssert.AreEqual(new List<LoadSaveHelper.SlotNum>() { LoadSaveHelper.SlotNum.First },LoadSaveHelper.GetOccupiedSlots());

            LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.Second);

            CollectionAssert.AreEqual(new List<LoadSaveHelper.SlotNum>() { LoadSaveHelper.SlotNum.First, LoadSaveHelper.SlotNum.Second }, LoadSaveHelper.GetOccupiedSlots());

            LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.Third);

            CollectionAssert.AreEqual(new List<LoadSaveHelper.SlotNum>() { LoadSaveHelper.SlotNum.First, LoadSaveHelper.SlotNum.Second, LoadSaveHelper.SlotNum.Third }, LoadSaveHelper.GetOccupiedSlots());

            LoadSaveHelper.EraseAllSlots();
        }

        [Test]
        public static void EraseASlotTest()
        {
            LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.First);

            LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.Second);

            LoadSaveHelper.CompleteALevel(1, LoadSaveHelper.SlotNum.Third);

            LoadSaveHelper.EraseASlot(LoadSaveHelper.SlotNum.First);
            
            CollectionAssert.AreEqual(new List<int>(), LoadSaveHelper.GetSlot(LoadSaveHelper.SlotNum.First));

            LoadSaveHelper.EraseASlot(LoadSaveHelper.SlotNum.Second);

            CollectionAssert.AreEqual(new List<int>(), LoadSaveHelper.GetSlot(LoadSaveHelper.SlotNum.Second));

            LoadSaveHelper.EraseASlot(LoadSaveHelper.SlotNum.Third);

            CollectionAssert.AreEqual(new List<int>(), LoadSaveHelper.GetSlot(LoadSaveHelper.SlotNum.Third));
        }

        [TearDown]
        public void Cleanup()
        {
            LoadSaveHelper.EraseAllSlots();
        }
    }
}
