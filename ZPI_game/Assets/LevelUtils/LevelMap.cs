
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LevelUtils
{
    public static class LevelMap
    {
        public const string JSON_FILE_NAME = "Assets/LevelUtils/levels.json";
        public const string JSON_FILE_NAME_TESTS = "Assets/LevelUtils/Tests/levels.json";
        private static LoadSaveHelper.SlotNum currSlot;
        private static List<LevelButtonInfo> ListOfLevels { get; set; }
        public static void LoadTestConfiguration(LoadSaveHelper.SlotNum slotNum)
        {
            currSlot = slotNum;
            ListOfLevels = LoadFromJson(JSON_FILE_NAME_TESTS);
        }
        public static void SynchronizeSlotNumber(LoadSaveHelper.SlotNum slotNum)
        {
            if (ListOfLevels == null || currSlot != slotNum)
            {
                currSlot = slotNum;
                ListOfLevels = LoadFromJson(JSON_FILE_NAME);
            }
        }
        private static List<LevelButtonInfo> LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            string jsonFile = File.ReadAllText(filePath);

            var parsedJson = JsonSerializer.Deserialize<List<LevelInfoJson>>(jsonFile);
            List<int> completed = LoadSaveHelper.GetSlot(currSlot);
            Dictionary<int, LevelButtonInfo> idToObjMap = new Dictionary<int, LevelButtonInfo>();
            List<LevelButtonInfo> levelButtons = parsedJson.Select(
                lvlInfoJson => {
                    LevelButtonInfo currLvlInfo = new LevelButtonInfo(lvlInfoJson.GameObjectName, lvlInfoJson.LevelName, lvlInfoJson.LevelNumber, completed.Contains(lvlInfoJson.LevelNumber), null);
                    idToObjMap.Add(lvlInfoJson.LevelNumber, currLvlInfo);
                    return currLvlInfo;
                }).ToList();

            levelButtons.ForEach(currLvlInfo => {
                LevelInfoJson currLvlJson = parsedJson.Where(lvlInfoJson => currLvlInfo.LevelName == lvlInfoJson.LevelName).First();
                currLvlInfo.PrevLevels = currLvlJson.PrevLevels != null ? currLvlJson.PrevLevels.Select(lvlNum => idToObjMap[lvlNum]).ToList() : null;
            });

            return levelButtons;
        }
        public static void CompleteALevel(string levelName, LoadSaveHelper.SlotNum slotNum)
        {
            SynchronizeSlotNumber(slotNum);
            LoadSaveHelper.CompleteALevel(ListOfLevels.Where(lvl => lvl.LevelName == levelName).First().LevelNumber, slotNum);
            List<int> completed = LoadSaveHelper.GetSlot(slotNum);
            ListOfLevels.ForEach(lvl => lvl.IsFinished = completed.Contains(lvl.LevelNumber));
        }
        public static List<LevelButtonInfo> GetListOfLevels(LoadSaveHelper.SlotNum slotNum)
        {
            SynchronizeSlotNumber(slotNum);
            return ListOfLevels;
        }
        public static bool IsLevelDone(string gameObjectName, LoadSaveHelper.SlotNum slotNum)
        {
            SynchronizeSlotNumber(slotNum);
            return ListOfLevels.Where(lvl => lvl.GameObjectName == gameObjectName).First().IsFinished;
        }
        public static List<string> GetPrevGameObjectNames(string gameObjectName, LoadSaveHelper.SlotNum slotNum)
        {
            SynchronizeSlotNumber(slotNum);
            List<LevelButtonInfo> prevLevels = ListOfLevels.Where(lvl => lvl.GameObjectName == gameObjectName).First().PrevLevels;
            if (prevLevels == null)
            {
                return new List<string>();
            }
            return prevLevels.Select(prevLvl => prevLvl.GameObjectName).ToList();
        }
    }
    class LevelInfoJson
    {
        public string GameObjectName { get; set; }
        public string LevelName { get; set; }
        public int LevelNumber { get; set; }
        public List<int> PrevLevels { get; set; }
    }
}

