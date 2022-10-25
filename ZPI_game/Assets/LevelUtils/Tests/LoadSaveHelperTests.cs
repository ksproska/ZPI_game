using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using UnityEngine.TestTools;
using UnityEngine;
using System.Collections;

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
        private void SetUpLevelUtilsObjects()
        {
            GameObject go = new GameObject();
            LoadSaveHelper loadSaveHelper = go.AddComponent<LoadSaveHelper>();
            LoadSaveHelper.Instance = loadSaveHelper;
            loadSaveHelper.LoadTestConfiguration();
        }
        
        [UnityTest]
        public IEnumerator TestCompleteALevel()
        {
            SetUpLevelUtilsObjects();

            LoadSaveHelper.Instance.EraseAllSlots();
            
            List<TestCase> testCases = new List<TestCase>()
            {
                new TestCase(new List<int>() {1, 2}),
                new TestCase(new List<int>() {1, 2, 3, 4, 5}),
                new TestCase(new List<int>() {1, 2, 3})
            };

            LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.First);
            LoadSaveHelper.Instance.CompleteALevel(2, LoadSaveHelper.SlotNum.First);


            LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.Instance.CompleteALevel(2, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.Instance.CompleteALevel(3, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.Instance.CompleteALevel(4, LoadSaveHelper.SlotNum.Second);
            LoadSaveHelper.Instance.CompleteALevel(5, LoadSaveHelper.SlotNum.Second);

            LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.Third);
            LoadSaveHelper.Instance.CompleteALevel(2, LoadSaveHelper.SlotNum.Third);
            LoadSaveHelper.Instance.CompleteALevel(3, LoadSaveHelper.SlotNum.Third);
            Assert.Throws<ArgumentException>(() => LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.First));
            var jsonStructuredDict = new Dictionary<string, List<Dictionary<string, List<int>>>>();
            var listOfSlots = new List<Dictionary<string, List<int>>>();
            foreach (List<int> levels in testCases.Select(el => el.savedLevels))
            {
                var jsonDict = new Dictionary<string, List<int>>();
                jsonDict.Add("levels_completed", levels);
                listOfSlots.Add(jsonDict);
            }
            jsonStructuredDict.Add("slots", listOfSlots);

            string jsonFile = File.ReadAllText(LoadSaveHelper.JSON_FILE_NAME_TESTS);
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, List<int>>>>>(jsonFile);
            CollectionAssert.AreEquivalent(jsonStructuredDict, parsedJson);
            LoadSaveHelper.Instance.EraseAllSlots();
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestGetSlot()
        {
            SetUpLevelUtilsObjects();

            List<TestCase> testCases = new List<TestCase>()
            {
                new TestCase(new List<int>() {}),
                new TestCase(new List<int>() {}),
                new TestCase(new List<int>() {})
            };
            List<int> firstSlot = LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.First);
            List<int> secondSlot = LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Second);
            List<int> thirdSlot = LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Third);

            CollectionAssert.AreEqual(testCases[0].savedLevels, firstSlot);
            CollectionAssert.AreEqual(testCases[1].savedLevels, secondSlot);
            CollectionAssert.AreEqual(testCases[2].savedLevels, thirdSlot);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetOccupiedSlotsTest()
        {
            SetUpLevelUtilsObjects();
            LoadSaveHelper.Instance.EraseAllSlots();

            Assert.AreEqual(0, LoadSaveHelper.Instance.GetOccupiedSlots().Count);

            LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.First);
            LoadSaveHelper.Instance.CompleteALevel(2, LoadSaveHelper.SlotNum.First);

            CollectionAssert.AreEqual(new List<LoadSaveHelper.SlotNum>() { LoadSaveHelper.SlotNum.First },LoadSaveHelper.Instance.GetOccupiedSlots());

            LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.Second);

            CollectionAssert.AreEqual(new List<LoadSaveHelper.SlotNum>() { LoadSaveHelper.SlotNum.First, LoadSaveHelper.SlotNum.Second }, LoadSaveHelper.Instance.GetOccupiedSlots());

            LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.Third);

            CollectionAssert.AreEqual(new List<LoadSaveHelper.SlotNum>() { LoadSaveHelper.SlotNum.First, LoadSaveHelper.SlotNum.Second, LoadSaveHelper.SlotNum.Third }, LoadSaveHelper.Instance.GetOccupiedSlots());

            LoadSaveHelper.Instance.EraseAllSlots();

            yield return null;
        }

        [UnityTest]
        public IEnumerator EraseASlotTest()
        {
            SetUpLevelUtilsObjects();
            LoadSaveHelper.Instance.EraseAllSlots();

            LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.First);

            LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.Second);

            LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.Third);

            LoadSaveHelper.Instance.EraseASlot(LoadSaveHelper.SlotNum.First);
            
            CollectionAssert.AreEqual(new List<int>(), LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.First));

            LoadSaveHelper.Instance.EraseASlot(LoadSaveHelper.SlotNum.Second);

            CollectionAssert.AreEqual(new List<int>(), LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Second));

            LoadSaveHelper.Instance.EraseASlot(LoadSaveHelper.SlotNum.Third);

            CollectionAssert.AreEqual(new List<int>(), LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Third));

            yield return null;
        }

    }
}
