using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LevelUtils
{
    enum SlotNumber
    {
        First,
        Second,
        Third
    }
    public static class LevelMap
    {
        private static List<LevelButtonInfo> ListOfLevels { get; set; }
        
        private static List<LevelButtonInfo> LoadFromJson(string jsonText)
        {
            var parsedJson = JsonSerializer.Deserialize<List<LevelInfoJson>>(jsonText);
            List<LevelButtonInfo> levelButtons = parsedJson.Select(lvlInfoJson => new LevelButtonInfo(lvlInfoJson.GameObjectName, lvlInfoJson.LevelNumber, false, null)).ToList();

        }
        public static List<LevelButtonInfo> GetListOfLevels()
        {
            if (ListOfLevels != null)
            {
                return ListOfLevels;
            }

            LoadFromJson("");
        }

    }
}

