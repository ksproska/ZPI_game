using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace LevelUtils
{
    [TestFixture]
    public class LevelMapTests
    {
        class TestCase
        {
            public readonly List<List<LevelButtonInfo>> levelList;
            public TestCase(List<List<LevelButtonInfo>> levelList)
            {
                this.levelList = levelList;
            }
        }
        private List<List<LevelButtonInfo>> GetTestConfig()
        {
            var levelList = new List<List<LevelButtonInfo>>()
            {
                new List<LevelButtonInfo>() { new LevelButtonInfo("bttn1", "lvl1", 1, false, null),
                    new LevelButtonInfo("bttn2", "lvl2", 2, false, new List<LevelButtonInfo>() { new LevelButtonInfo("bttn1", "lvl1", 1, false, null)} ),
                    new LevelButtonInfo("bttn3","lvl3", 3, false, new List<LevelButtonInfo>()),
                    new LevelButtonInfo("bttn4","lvl4", 4, false, null), new LevelButtonInfo("bttn5", "lvl5", 5, false, null),
                    new LevelButtonInfo("bttn6", "lvl6", 6, false, null)}

            };
            levelList[0][2].PrevLevels = new List<LevelButtonInfo>() { levelList[0][1] };
            levelList[0][3].PrevLevels = new List<LevelButtonInfo>() { levelList[0][2] };
            levelList[0][4].PrevLevels = new List<LevelButtonInfo>() { levelList[0][2] };
            levelList[0][5].PrevLevels = new List<LevelButtonInfo>() { levelList[0][3], levelList[0][4] };
            return levelList;
        }
        private void SetUpLevelUtilsObjects()
        {
            GameObject go = new GameObject();
            LevelMap levelMap = go.AddComponent<LevelMap>();
            LevelMap.Instance = levelMap;
            LoadSaveHelper loadSaveHelper = go.AddComponent<LoadSaveHelper>();
            LoadSaveHelper.Instance = loadSaveHelper;
            loadSaveHelper.LoadTestConfiguration();
            levelMap.LoadTestConfiguration(LoadSaveHelper.SlotNum.Third);
        }
        [UnityTest, Order(1)]
        public IEnumerator GetListOfLevelsTest()
        {
            SetUpLevelUtilsObjects();

            var testCase = new TestCase(GetTestConfig());

            List<LevelButtonInfo> lvlInfos = LevelMap.Instance.GetListOfLevels(LoadSaveHelper.SlotNum.Third);
            CollectionAssert.AreEquivalent(testCase.levelList[0], lvlInfos);
            yield return null;

            
        }
        [UnityTest, Order(3)]
        public IEnumerator IsLevelDoneTest()
        {
            SetUpLevelUtilsObjects();

            List<LevelButtonInfo> lvlInfos = LevelMap.Instance.GetListOfLevels(LoadSaveHelper.SlotNum.Third);
            foreach (LevelButtonInfo lvlButtonInfo in lvlInfos)
            {
                Assert.AreEqual(lvlButtonInfo.IsFinished, LevelMap.Instance.IsLevelDone(lvlButtonInfo.GameObjectName, LoadSaveHelper.SlotNum.Third));
            }
            yield return null;
        }
        [UnityTest, Order(1)]
        public IEnumerator GetPrevGameObjectNamesTest()
        {
            SetUpLevelUtilsObjects();

            var testCase = new TestCase(GetTestConfig());
            foreach (LevelButtonInfo lvl in testCase.levelList[0])
            {
                LevelMap.Instance.GetPrevGameObjectNames(lvl.GameObjectName, LoadSaveHelper.SlotNum.Third).
                    ForEach(lvlName => Assert.Contains(lvlName, LevelMap.Instance.GetListOfLevels(LoadSaveHelper.SlotNum.Third).Select(lvl => lvl.GameObjectName).ToList()));
            }
            yield return null;
        }
        [UnityTest, Order(2)]
        public IEnumerator CompleteALevelTest()
        {
            SetUpLevelUtilsObjects();

            LoadSaveHelper.Instance.EraseAllSlots();
            List<LevelButtonInfo> lvlInfos = LevelMap.Instance.GetListOfLevels(LoadSaveHelper.SlotNum.Third);
            Assert.AreEqual(false, lvlInfos[0].IsFinished);
            LevelMap.Instance.CompleteALevel(lvlInfos[0].LevelName, LoadSaveHelper.SlotNum.Third);
            Assert.AreEqual(true, lvlInfos[0].IsFinished);
            //Assert.Throws<ArgumentException>(() => LevelMap.Instance.CompleteALevel(lvlInfos[0].LevelName, LoadSaveHelper.SlotNum.Third));
            yield return null;
        }
        [UnityTearDown]
        public void Cleanup()
        {
            LoadSaveHelper.Instance.EraseAllSlots();
        }
    }
}
