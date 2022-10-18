
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LevelUtils
{
    public static class LevelMap
    {
        public const string JSON_FILE_NAME = "..\\..\\..\\levels.json";
        private static LoadSaveHelper.SlotNum currSlot;
        private static List<LevelButtonInfo> ListOfLevels { get; set; }
        private static List<LevelButtonInfo> LoadFromJson()
        {
            if (!File.Exists(JSON_FILE_NAME))
                throw new FileNotFoundException(JSON_FILE_NAME);

            string jsonFile = File.ReadAllText(JSON_FILE_NAME);

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
            LoadSaveHelper.CompleteALevel(ListOfLevels.Where(lvl => lvl.LevelName == levelName).First().LevelNumber, slotNum);
            List<int> completed = LoadSaveHelper.GetSlot(slotNum);
            ListOfLevels.ForEach(lvl => lvl.IsFinished = completed.Contains(lvl.LevelNumber));
        }
        public static List<LevelButtonInfo> GetListOfLevels(LoadSaveHelper.SlotNum slotNum)
        {
            if (ListOfLevels != null && currSlot == slotNum)
            {
                return ListOfLevels;
            }
            currSlot = slotNum;
            ListOfLevels = LoadFromJson();
            return ListOfLevels;
        }

    }
}

