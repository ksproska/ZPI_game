
using System;
using Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelUtils
{
    public class LevelMap : MonoBehaviour
    {
        public const string JSON_FILE_NAME = "/LevelUtils/levels.json";
        public const string JSON_FILE_NAME_TESTS = "Assets/LevelUtils/Tests/levels.json";
        private LoadSaveHelper.SlotNum _currSlot;
        private List<LevelButtonInfo> ListOfLevels { get; set; }
        public static LevelMap Instance { get; set; }
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
        }
        public void LoadTestConfiguration(LoadSaveHelper.SlotNum slotNum)
        {
            _currSlot = slotNum;
            ListOfLevels = LoadFromJson(JSON_FILE_NAME_TESTS);
        }
        public void SynchronizeSlotNumber(LoadSaveHelper.SlotNum slotNum)
        {
            if (ListOfLevels == null || _currSlot != slotNum)
            {
                _currSlot = slotNum;
                ListOfLevels = LoadLevels(); // LoadFromJson(Application.persistentDataPath + JSON_FILE_NAME);
            }
        }
        private void CreateLevelMap(string filePath)
        {
            List<LevelInfoJson> defLevelButtonInfos = new List<LevelInfoJson>() {
                    new LevelInfoJson("StoryBeginning", "StoryBeginning", 1, null),
                    new LevelInfoJson("lvl_learn_1", "lvl_learn_1", 2, new List<int>() {1}),
                    new LevelInfoJson("lvl_number_1", "lvl_number_1", 3, new List<int>() {2}),
                    new LevelInfoJson("lvl_number_2","lvl_number_2", 4, new List<int>() {3}),
                    new LevelInfoJson("lvl_learn_2","lvl_learn_2", 5, new List<int>() {4}),
                    new LevelInfoJson("lvl_number_3", "lvl_number_3", 6, new List<int>() {5}),
                    new LevelInfoJson("lvl_number_4", "lvl_number_4", 7, new List<int>() {5}),
                    new LevelInfoJson("lvl_number_5", "lvl_number_5", 8, new List<int>() {6, 7}),
                    new LevelInfoJson("lvl_cutscene_1", "lvl_cutscene_1", 9, new List<int>() {8})};
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonLevelButtonInfos = JsonSerializer.Serialize(defLevelButtonInfos, options);
            File.WriteAllText(filePath, jsonLevelButtonInfos);
        }
        private List<LevelButtonInfo> LoadLevels()
        {
            List<string> scenes = GetScenes().Where(scn => IsNavigableFromMap(scn)).ToList();
            List<int> completed = LoadSaveHelper.Instance.GetSlot(_currSlot);
            List<LevelButtonInfo> levelInfos = scenes.Select(scn => new LevelButtonInfo(GetClearMapName(scn), GetClearMapName(scn), GetSceneNumber(scn), true, null)).ToList();
            levelInfos.ForEach(lvlInfo => lvlInfo.IsFinished = completed.Contains(lvlInfo.LevelNumber));
            levelInfos.Join(scenes, levelInf => levelInf.LevelName, scn => GetClearMapName(scn), (levelInf, scn) => new { LevelInfo = levelInf, Scene = scn }).ToList().
                ForEach(elem => {
                    List<int> prevLevelNums = GetPreviousMapLevels(elem.Scene);
                    elem.LevelInfo.PrevLevels = levelInfos.Where(lvlInfo => prevLevelNums.Contains(lvlInfo.LevelNumber)).ToList();
                });
            return levelInfos;
        }
        private List<string> GetScenes()
        {
            List<string> scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(Path.GetFileNameWithoutExtension(scene.path));
                }
            }
            return scenes;
        }
        private List<LevelButtonInfo> LoadFromJson(string filePath)
        {
            new FileInfo(filePath).Directory.Create();

            if (!File.Exists(filePath))
                CreateLevelMap(filePath);

            string jsonFile = File.ReadAllText(filePath);

            var parsedJson = JsonSerializer.Deserialize<List<LevelInfoJson>>(jsonFile);
            List<int> completed = LoadSaveHelper.Instance.GetSlot(_currSlot);
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
        public void CompleteALevel(string levelName, LoadSaveHelper.SlotNum slotNum)
        {
            SynchronizeSlotNumber(slotNum);
            LoadSaveHelper.Instance.CompleteALevel(ListOfLevels.Where(lvl => lvl.LevelName == levelName).First().LevelNumber, slotNum);
            List<int> completed = LoadSaveHelper.Instance.GetSlot(slotNum);
            ListOfLevels.ForEach(lvl => lvl.IsFinished = completed.Contains(lvl.LevelNumber));
        }
        public List<LevelButtonInfo> GetListOfLevels(LoadSaveHelper.SlotNum slotNum)
        {
            SynchronizeSlotNumber(slotNum);
            return ListOfLevels;
        }
        public bool IsLevelDone(string gameObjectName, LoadSaveHelper.SlotNum slotNum)
        {
            SynchronizeSlotNumber(slotNum);
            return ListOfLevels.Where(lvl => lvl.GameObjectName == gameObjectName).First().IsFinished;
        }
        public List<string> GetPrevGameObjectNames(string gameObjectName, LoadSaveHelper.SlotNum slotNum)
        {
            SynchronizeSlotNumber(slotNum);
            List<LevelButtonInfo> prevLevels = ListOfLevels.Where(lvl => lvl.GameObjectName == gameObjectName).First().PrevLevels;
            if (prevLevels == null)
            {
                return new List<string>();
            }
            return prevLevels.Select(prevLvl => prevLvl.GameObjectName).ToList();
        }
        
        public static bool IsNavigableFromMap(string name)
        {
            return name.StartsWith("map");
        }
        
        public static string GetClearMapName(string name)
        {
            if (!IsNavigableFromMap(name))
                throw new NotMapNavigableException($"Scene {name} is not navigable from world map.");
            var splitName = name.Split('_');
            if (splitName.Length < 3)
                throw new InvalidMapNavigableName($"Scene {name} has an invalid name " +
                                                  $"structure for a map navigable scene. Correct " +
                                                  $"structure should look like: " +
                                                  $"\"map_1_SceneName\"");
            return splitName[2];
        }
        
        public static List<int> GetPreviousMapLevels(string name)
        {
            if (!IsNavigableFromMap(name))
                throw new NotMapNavigableException($"Scene {name} is not navigable from world map.");
            var split = name.Split('_');
            if (split.Length < 3)
                throw new InvalidMapNavigableName($"Scene {name} has an invalid name " +
                                                  $"structure for a map navigable scene. Correct " +
                                                  $"structure should look like: " +
                                                  $"\"map_1_SceneName\"");
            if (split.Length == 2) return new List<int>();
            return split.Skip(3).Select(int.Parse).ToList();
        }

        public static int GetSceneNumber(string scene)
        {
            if (IsNavigableFromMap(scene))
                throw new NotMapNavigableException($"Scene {scene} is not navigable from world map.");
            var split = scene.Split('_');
            if (split.Length < 3)
                throw new InvalidMapNavigableName($"Scene {scene} has an invalid name " +
                                                  $"structure for a map navigable scene. Correct " +
                                                  $"structure should look like: " +
                                                  $"\"map_1_SceneName\"");
            return int.Parse(split[1]);
        }

        public static string MakeMapSceneName(string clearName, int number, IEnumerable<int> previousLevels)
        {
            string previousLevelsChain = string.Join('_', previousLevels);
            var ret = $"map_{number}_{clearName}";
            if (previousLevelsChain.Length != 0)
            {
                ret += $"_{previousLevelsChain}";
            }
            return ret;
        }
    }
    class LevelInfoJson
    {
        public string GameObjectName { get; set; }
        public string LevelName { get; set; }
        public int LevelNumber { get; set; }
        public List<int> PrevLevels { get; set; }

        public LevelInfoJson(string gameObjectName, string levelName, int levelNumber, List<int> prevLevels)
        {
            GameObjectName = gameObjectName;
            LevelName = levelName;
            LevelNumber = levelNumber;
            PrevLevels = prevLevels;
        }
    }
    
    public class NotMapNavigableException : Exception
    {
        public NotMapNavigableException(string message = "") : base(message)
        {
            
        }
    }

    public class InvalidMapNavigableName : Exception
    {
        public InvalidMapNavigableName(string message = "") : base(message)
        {
            
        }
    }
}

