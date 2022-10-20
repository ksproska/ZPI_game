using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace LevelUtils
{
    public static class LoadSaveHelper
    {
        public const int SLOT_NUMBER = 3;
        public const string JSON_FILE_NAME = "Assets\\LevelUtils\\save_slots.json";
        public enum SlotNum
        {
            First,
            Second,
            Third
        }

        private static List<int>[] slots = GetCompletedLevels();
        private static List<int>[] GetCompletedLevels()
        {
            if (!File.Exists(JSON_FILE_NAME))
                throw new FileNotFoundException(JSON_FILE_NAME);

            string jsonFile = File.ReadAllText(JSON_FILE_NAME);
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, List<int>>>>>(jsonFile);
            return new List<int>[]
            {
                parsedJson["slots"][0]["levels_completed"],
                parsedJson["slots"][1]["levels_completed"],
                parsedJson["slots"][2]["levels_completed"]
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
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonText = JsonSerializer.Serialize(jsonStructuredDict, options);
            File.WriteAllText(JSON_FILE_NAME, jsonText);
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
            SaveGameState();
        }
        public static void EraseASlot(SlotNum slotNum)
        {
            switch (slotNum)
            {
                case SlotNum.First:
                    slots[0] = new List<int>();
                    break;
                case SlotNum.Second:
                    slots[1] = new List<int>();
                    break;
                case SlotNum.Third:
                    slots[2] = new List<int>();
                    break;
            }
            SaveGameState();
        }
        public static void EraseAllSlots()
        {
            for(int slotNum = 0; slotNum < SLOT_NUMBER; slotNum++)
            {
                slots[slotNum] = new List<int>();
            }
            SaveGameState();
        }
        public static List<SlotNum> GetOccupiedSlots()
        {
            List<SlotNum> occSlots = new List<SlotNum>();
            if (slots[0].Count > 0)
            {
                occSlots.Add(SlotNum.First);
            }
            if (slots[1].Count > 0)
            {
                occSlots.Add(SlotNum.Second);
            }
            if (slots[2].Count > 0)
            {
                occSlots.Add(SlotNum.Third);
            }
            return occSlots;
        }
    }
}

