using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace LevelUtils
{
    public static class LoadSaveHelper
    {
        //public const int SLOT_NUMBER = 3;
        public const string JSON_FILE_NAME = "save_slots.json";
        public enum SlotNum
        {
            First,
            Second,
            Third
        }

        private static List<int>[] slots = GetCompletedLevels();
        private static List<int>[] GetCompletedLevels()
        {
            string jsonFile = File.ReadAllText("");
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, List<int>>>>>(jsonFile);
            return new List<int>[]
            {
                parsedJson["slots"][0]["levels_completed"],
                parsedJson["slots"][0]["levels_completed"],
                parsedJson["slots"][0]["levels_completed"]
            };
        }
        public static List<int> GetSlot(SlotNum slot)
        {
            List<int> result = null;
            switch (slot)
            {
                case SlotNum.First:
                    result = slots[0];
                    break;
                case SlotNum.Second:
                    result = slots[1];
                    break;
                case SlotNum.Third:
                    result = slots[2];
                    break;
            }
            return result;
        }
        public static void SaveGameState()
        {
            var jsonStructuredDict = new Dictionary<string, List<Dictionary<string, List<int>>>>();
            var listOfSlots = new List<Dictionary<string, List<int>>>();
            foreach(List<int> levels in slots)
            {
                var jsonDict = new Dictionary<string, List<int>>();
                jsonDict.Add("levels_completed", levels);
                listOfSlots.Add(jsonDict);
            }
            jsonStructuredDict.Add("slots", listOfSlots);
            string jsonText = JsonSerializer.Serialize(jsonStructuredDict);
            File.WriteAllText("", jsonText);
        }
        public static void CompleteALevel(int LevelName, SlotNum slot)
        {
            switch (slot)
            {
                case SlotNum.First:
                    if (slots[0].Contains(LevelName))
                    {
                        throw new ArgumentException("Level is already completed!!");
                    }
                    slots[0].Add(LevelName);
                    break;
                case SlotNum.Second:
                    if (slots[1].Contains(LevelName))
                    {
                        throw new ArgumentException("Level is already completed!!");
                    }
                    slots[1].Add(LevelName);
                    break;
                case SlotNum.Third:
                    if (slots[2].Contains(LevelName))
                    {
                        throw new ArgumentException("Level is already completed!!");
                    }
                    slots[2].Add(LevelName);
                    break;
            }
        }
    }
}

