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
using Maps;

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
            loadSaveHelper.EraseAllSlots();
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
            //Assert.Throws<ArgumentException>(() => LoadSaveHelper.Instance.CompleteALevel(1, LoadSaveHelper.SlotNum.First));
            List<SavedSlotInfo> savedSlots = new List<SavedSlotInfo>()
            {
                new SavedSlotInfo()
                {
                    CompletedLevels = testCases[0].savedLevels,
                    BestScores = new float[] { -1f, -1f, -1f, -1f, -1f, -1f },
                    Sandbox = new Sandbox() { Selector = LoadSaveHelper.DEFAULT_SELECTOR, Mutator = LoadSaveHelper.DEFAULT_MUTATOR, Crosser = LoadSaveHelper.DEFAULT_CROSSER, CrossoverProbab = 0.5f, MutationProb = 0.5f, CurrentBestScore = -1f, PopulationSize = LoadSaveHelper.DEFAULT_POP_SIZE, UserMap = null }
                },
                new SavedSlotInfo()
                {
                    CompletedLevels = testCases[1].savedLevels,
                    BestScores = new float[] { -1f, -1f, -1f, -1f, -1f, -1f },
                    Sandbox = new Sandbox() { Selector = LoadSaveHelper.DEFAULT_SELECTOR, Mutator = LoadSaveHelper.DEFAULT_MUTATOR, Crosser = LoadSaveHelper.DEFAULT_CROSSER, CrossoverProbab = 0.5f, MutationProb = 0.5f, CurrentBestScore = -1f, PopulationSize = LoadSaveHelper.DEFAULT_POP_SIZE, UserMap = null }
                },
                new SavedSlotInfo()
                {
                    CompletedLevels = testCases[2].savedLevels,
                    BestScores = new float[] { -1f, -1f, -1f, -1f, -1f, -1f },
                    Sandbox = new Sandbox() { Selector = LoadSaveHelper.DEFAULT_SELECTOR, Mutator = LoadSaveHelper.DEFAULT_MUTATOR, Crosser = LoadSaveHelper.DEFAULT_CROSSER, CrossoverProbab = 0.5f, MutationProb = 0.5f, CurrentBestScore = -1f, PopulationSize = LoadSaveHelper.DEFAULT_POP_SIZE, UserMap = null }
                },
            };

            string jsonFile = File.ReadAllText(LoadSaveHelper.JSON_FILE_NAME_TESTS);
            var parsedJson = JsonSerializer.Deserialize<List<SavedSlotInfo>>(jsonFile);
            CollectionAssert.AreEquivalent(savedSlots, parsedJson);

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
            List<int> firstSlot = LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.First).CompletedLevels;
            List<int> secondSlot = LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Second).CompletedLevels;
            List<int> thirdSlot = LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Third).CompletedLevels;

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
            
            CollectionAssert.AreEqual(new List<int>(), LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.First).CompletedLevels);

            LoadSaveHelper.Instance.EraseASlot(LoadSaveHelper.SlotNum.Second);

            CollectionAssert.AreEqual(new List<int>(), LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Second).CompletedLevels);

            LoadSaveHelper.Instance.EraseASlot(LoadSaveHelper.SlotNum.Third);

            CollectionAssert.AreEqual(new List<int>(), LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Third).CompletedLevels);

            //LoadSaveHelper.Instance.EraseAllSlots();

            yield return null;
        }

    }
}
