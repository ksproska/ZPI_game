using System.Collections;
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
            //Console.WriteLine("Is this JSON null?: " + (parsedJson == null));
            List<int> completed = LoadSaveHelper.GetSlot(currSlot);
            Dictionary<int, LevelButtonInfo> idToObjMap = new Dictionary<int, LevelButtonInfo>();
            List<LevelButtonInfo> levelButtons = parsedJson.Select(
                lvlInfoJson => { 
                    LevelButtonInfo currLvlInfo = new LevelButtonInfo(lvlInfoJson.GameObjectName, lvlInfoJson.LevelNumber, completed.Contains(lvlInfoJson.LevelNumber), null);
                    idToObjMap.Add(lvlInfoJson.LevelNumber, currLvlInfo);
                    return currLvlInfo;
                }).ToList();
            
            levelButtons.ForEach(currLvlInfo => {
                LevelInfoJson currLvlJson = parsedJson.Where(lvlInfoJson => currLvlInfo.LevelNumber == lvlInfoJson.LevelNumber).First();
                currLvlInfo.PrevLevels = currLvlJson.PrevLevels != null ? currLvlJson.PrevLevels.Select(lvlNum => idToObjMap[lvlNum]).ToList() : null;
                });
            //levelButtons.ForEach(elem => Console.WriteLine($"{elem.LevelName} {elem.LevelNumber} {(elem.PrevLevels != null ? elem.PrevLevels.Count : 0)}"));
            return levelButtons;
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

