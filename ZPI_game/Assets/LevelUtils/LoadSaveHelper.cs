using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;
using Maps;
using System.Linq;

namespace LevelUtils
{
    public class LoadSaveHelper : MonoBehaviour
    {
        public const int SLOT_NUMBER = 3;
        public const string JSON_FILE_NAME = "/LevelUtils/save_slots.json";
        public const string JSON_FILE_NAME_TESTS = "Assets\\LevelUtils\\Tests\\save_slots.json";
        public const Selector DEFAULT_SELECTOR = Selector.Tournament;
        public const Mutator DEFAULT_MUTATOR = Mutator.RSM;
        public const Crosser DEFAULT_CROSSER = Crosser.OX;
        public const int DEFAULT_POP_SIZE = 10;
        public enum SlotNum
        {
            First,
            Second,
            Third
        }
        public static LoadSaveHelper Instance { get; set; }
        private SavedSlotInfo[] _slots;
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
            List<SavedSlotInfo> emptySlots = new List<SavedSlotInfo>() 
            {
                GetDefSlot(), 
                GetDefSlot(),
                GetDefSlot()
            };

            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringSlots = JsonSerializer.Serialize(emptySlots, options);
            File.WriteAllText(filePath, jsonStringSlots);
        }
        private SavedSlotInfo[] GetCompletedLevels(string filePath)
        {
            new FileInfo(filePath).Directory.Create();

            if (!File.Exists(filePath))
                CreateDefSlots(filePath);

            string jsonFile = File.ReadAllText(filePath);
            var parsedJson = JsonSerializer.Deserialize<List<SavedSlotInfo>>(jsonFile);
            return parsedJson.ToArray();
        }
        public void LoadTestConfiguration()
        {
            _slots = GetCompletedLevels(JSON_FILE_NAME_TESTS);
            _isTestConfig = true;
        }
        public SavedSlotInfo GetSlot(SlotNum slot)
        {
            SavedSlotInfo result = null;
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
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonText = JsonSerializer.Serialize(_slots, options);
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
                    if (_slots[0].CompletedLevels.Contains(LevelName))
                    {
                        return;
                        //throw new ArgumentException("Level is already completed!!");
                    }
                    _slots[0].CompletedLevels.Add(LevelName);
                    break;
                case SlotNum.Second:
                    if (_slots[1].CompletedLevels.Contains(LevelName))
                    {
                        return;
                        //throw new ArgumentException("Level is already completed!!");
                    }
                    _slots[1].CompletedLevels.Add(LevelName);
                    break;
                case SlotNum.Third:
                    if (_slots[2].CompletedLevels.Contains(LevelName))
                    {
                        return;
                        //throw new ArgumentException("Level is already completed!!");
                    }
                    _slots[2].CompletedLevels.Add(LevelName);
                    break;
            }
            SaveGameState();
        }
        public void EraseASlot(SlotNum slotNum)
        {
            switch (slotNum)
            {
                case SlotNum.First:
                    _slots[0] = GetDefSlot();
                    break;
                case SlotNum.Second:
                    _slots[1] = GetDefSlot();
                    break;
                case SlotNum.Third:
                    _slots[2] = GetDefSlot();
                    break;
            }
            SaveGameState();
        }
        private SavedSlotInfo GetDefSlot()
        {
            return new SavedSlotInfo()
            {
                CompletedLevels = new List<int>(),
                BestScores = new float[] { -1f, -1f, -1f, -1f, -1f, -1f },
                Sandbox = new Sandbox() { Selector = DEFAULT_SELECTOR, Mutator = DEFAULT_MUTATOR, Crosser = DEFAULT_CROSSER, CrossoverProbab = 0.5f, MutationProb = 0.5f, CurrentBestScore = -1f, PopulationSize = DEFAULT_POP_SIZE, UserMap = null }
            };
        }
        public void EraseAllSlots()
        {
            for(int slotNum = 0; slotNum < SLOT_NUMBER; slotNum++)
            {
                _slots[slotNum] = GetDefSlot();
            }
            SaveGameState();
        }
        public List<SlotNum> GetOccupiedSlots()
        {
            List<SlotNum> occSlots = new List<SlotNum>();
            if (_slots[0].CompletedLevels.Count > 0)
            {
                occSlots.Add(SlotNum.First);
            }
            if (_slots[1].CompletedLevels.Count > 0)
            {
                occSlots.Add(SlotNum.Second);
            }
            if (_slots[2].CompletedLevels.Count > 0)
            {
                occSlots.Add(SlotNum.Third);
            }
            return occSlots;
        }
        
    }
    public enum Selector
    {
        Tournament,
        Roulette
    }
    public enum Mutator
    {
        Thrors,
        RSM
    }
    public enum Crosser
    {
        OX,
        PMX,
        CX
    }
    public class SavedSlotInfo
    {
        public List<int> CompletedLevels { get; set; }
        public float[] BestScores { get; set; }
        public Sandbox Sandbox { get; set; }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                SavedSlotInfo otherSlot = obj as SavedSlotInfo;
                return CompletedLevels.SequenceEqual(otherSlot.CompletedLevels)
                    && BestScores.SequenceEqual(otherSlot.BestScores)
                   && Sandbox.Equals(otherSlot.Sandbox);
            }
        }
    }
    public class Sandbox
    {
        public Selector Selector { get; set; }
        public Mutator Mutator { get; set; }
        public float MutationProb { get; set; }
        public Crosser Crosser { get; set; }
        public float CrossoverProbab { get; set; }
        public int PopulationSize { get; set; }
        public float CurrentBestScore { get; set; }
        public Map UserMap { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Sandbox otherSdbx = obj as Sandbox;
                return Selector == otherSdbx.Selector
                    && Mutator == otherSdbx.Mutator
                    && Crosser == otherSdbx.Crosser
                    && Math.Abs(MutationProb - otherSdbx.MutationProb) < 0.0000001f
                    && Math.Abs(CrossoverProbab - otherSdbx.CrossoverProbab) < 0.0000001f
                    && PopulationSize == otherSdbx.PopulationSize
                    && Math.Abs(CurrentBestScore - otherSdbx.CurrentBestScore) < 0.0000001f
                    && (UserMap == null && otherSdbx.UserMap == null || UserMap.Equals(otherSdbx.UserMap));
            }
        }
    }
}

