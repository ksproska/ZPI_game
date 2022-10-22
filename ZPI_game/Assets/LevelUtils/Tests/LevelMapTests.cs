// using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private static List<List<LevelButtonInfo>> GetTestConfig()
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
        [Test, Order(1)]
        public static void GetListOfLevelsTest()
        {
            var testCase = new TestCase(GetTestConfig());
            
            List<LevelButtonInfo> lvlInfos = LevelMap.GetListOfLevels(LoadSaveHelper.SlotNum.Third);
            
            CollectionAssert.AreEquivalent(testCase.levelList[0], lvlInfos);
        }
        [Test, Order(3)]
        public static void IsLevelDoneTest()
        {
            List<LevelButtonInfo> lvlInfos = LevelMap.GetListOfLevels(LoadSaveHelper.SlotNum.Third);
            foreach(LevelButtonInfo lvlButtonInfo in lvlInfos)
            {
                Assert.AreEqual(lvlButtonInfo.IsFinished, LevelMap.IsLevelDone(lvlButtonInfo.GameObjectName, LoadSaveHelper.SlotNum.Third));
            }
        }
        [Test, Order(1)]
        public static void GetPrevGameObjectNamesTest()
        {
            var testCase = new TestCase(GetTestConfig());
            foreach (LevelButtonInfo lvl in testCase.levelList[0])
            {
                LevelMap.GetPrevGameObjectNames(lvl.GameObjectName, LoadSaveHelper.SlotNum.Third).
                    ForEach(lvlName => Assert.Contains(lvlName, LevelMap.GetListOfLevels(LoadSaveHelper.SlotNum.Third).Select(lvl => lvl.GameObjectName).ToList()));
            }
        }
        [Test, Order(2)]
        public static void CompleteALevelTest()
        {
            LoadSaveHelper.EraseAllSlots();
            List<LevelButtonInfo> lvlInfos = LevelMap.GetListOfLevels(LoadSaveHelper.SlotNum.Third);
            Assert.AreEqual(false, lvlInfos[0].IsFinished);
            LevelMap.CompleteALevel(lvlInfos[0].LevelName, LoadSaveHelper.SlotNum.Third);
            Assert.AreEqual(true, lvlInfos[0].IsFinished);
            Assert.Throws<ArgumentException>(() => LevelMap.CompleteALevel(lvlInfos[0].LevelName, LoadSaveHelper.SlotNum.Third));
        }
        [TearDown]
        public void Cleanup()
        {
            LoadSaveHelper.EraseAllSlots();
        }
    }
}
