using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;

namespace LevelUtils
{
    public class LoadSaveHelper : MonoBehaviour
    {
        public const int SLOT_NUMBER = 3;
        public const string JSON_FILE_NAME = "/LevelUtils/save_slots.json";
        public const string JSON_FILE_NAME_TESTS = "Assets\\LevelUtils\\Tests\\save_slots.json";
        public enum SlotNum
        {
            First,
            Second,
            Third
        }
        public static LoadSaveHelper Instance { get; set; }
        private List<int>[] _slots;
        private bool _isTestConfig = false;
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            _slots = GetCompletedLevels(Application.persistentDataPath + JSON_FILE_NAME);
        }
        private void CreateDefSlots(string filePath)
        {
            List<List<int>> emptySlots = new List<List<int>>() { new List<int>(), new List<int>(), new List<int>() };

            var jsonStructuredDict = new Dictionary<string, List<Dictionary<string, List<int>>>>();
            var listOfSlots = new List<Dictionary<string, List<int>>>();
            foreach (List<int> levels in emptySlots)
            {
                var jsonDict = new Dictionary<string, List<int>>();
                jsonDict.Add("levels_completed", levels);
                listOfSlots.Add(jsonDict);
            }
            jsonStructuredDict.Add("slots", listOfSlots);

            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringSlots = JsonSerializer.Serialize(jsonStructuredDict, options);
            File.WriteAllText(filePath, jsonStringSlots);
        }
        private List<int>[] GetCompletedLevels(string filePath)
        {
            new FileInfo(filePath).Directory.Create();

            if (!File.Exists(filePath))
                CreateDefSlots(filePath);

            string jsonFile = File.ReadAllText(filePath);
            var parsedJson = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, List<int>>>>>(jsonFile);
            return new List<int>[]
            {
                parsedJson["slots"][0]["levels_completed"],
                parsedJson["slots"][1]["levels_completed"],
                parsedJson["slots"][2]["levels_completed"]
            };
        }
        public void LoadTestConfiguration()
        {
            _slots = GetCompletedLevels(JSON_FILE_NAME_TESTS);
            _isTestConfig = true;
        }
        public List<int> GetSlot(SlotNum slot)
        {
            List<int> result = null;
            switch (slot)
            {
                case SlotNum.First:
                    result = _slots[0];
                    break;
                case SlotNum.Second:
                    result = _slots[1];
                    break;
                case SlotNum.Third:
                    result = _slots[2];
                    break;
            }
            return result;
        }
        public void SaveGameState()
        {
            var jsonStructuredDict = new Dictionary<string, List<Dictionary<string, List<int>>>>();
            var listOfSlots = new List<Dictionary<string, List<int>>>();
            foreach(List<int> levels in _slots)
            {
                var jsonDict = new Dictionary<string, List<int>>();
                jsonDict.Add("levels_completed", levels);
                listOfSlots.Add(jsonDict);
            }
            jsonStructuredDict.Add("slots", listOfSlots);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonText = JsonSerializer.Serialize(jsonStructuredDict, options);
            if (_isTestConfig)
                File.WriteAllText(JSON_FILE_NAME_TESTS, jsonText);
            else
                File.WriteAllText(Application.persistentDataPath + JSON_FILE_NAME, jsonText);
        }
        public void CompleteALevel(int LevelName, SlotNum slot)
        {
            switch (slot)
            {
                case SlotNum.First:
                    if (_slots[0].Contains(LevelName))
                    {
                        throw new ArgumentException("Level is already completed!!");
                    }
                    _slots[0].Add(LevelName);
                    break;
                case SlotNum.Second:
                    if (_slots[1].Contains(LevelName))
                    {
                        throw new ArgumentException("Level is already completed!!");
                    }
                    _slots[1].Add(LevelName);
                    break;
                case SlotNum.Third:
                    if (_slots[2].Contains(LevelName))
                    {
                        throw new ArgumentException("Level is already completed!!");
                    }
                    _slots[2].Add(LevelName);
                    break;
            }
            SaveGameState();
        }
        public void EraseASlot(SlotNum slotNum)
        {
            switch (slotNum)
            {
                case SlotNum.First:
                    _slots[0] = new List<int>();
                    break;
                case SlotNum.Second:
                    _slots[1] = new List<int>();
                    break;
                case SlotNum.Third:
                    _slots[2] = new List<int>();
                    break;
            }
            SaveGameState();
        }
        public void EraseAllSlots()
        {
            for(int slotNum = 0; slotNum < SLOT_NUMBER; slotNum++)
            {
                _slots[slotNum] = new List<int>();
            }
            SaveGameState();
        }
        public List<SlotNum> GetOccupiedSlots()
        {
            List<SlotNum> occSlots = new List<SlotNum>();
            if (_slots[0].Count > 0)
            {
                occSlots.Add(SlotNum.First);
            }
            if (_slots[1].Count > 0)
            {
                occSlots.Add(SlotNum.Second);
            }
            if (_slots[2].Count > 0)
            {
                occSlots.Add(SlotNum.Third);
            }
            return occSlots;
        }
    }
}

