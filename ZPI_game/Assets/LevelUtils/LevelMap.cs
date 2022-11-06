
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
                ListOfLevels = LoadFromJson(Application.persistentDataPath + JSON_FILE_NAME);
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
            List<Scene> scenes = GetScenes().Where(scn => scn.IsNavigableFromMap()).ToList();
            List<int> completed = LoadSaveHelper.Instance.GetSlot(_currSlot);
            List<LevelButtonInfo> levelInfos = scenes.Select(scn => new LevelButtonInfo(scn.GetClearName(), scn.GetClearName(), scn.GetSceneNumber(), true, null)).ToList();
            levelInfos.ForEach(lvlInfo => lvlInfo.IsFinished = completed.Contains(lvlInfo.LevelNumber));
            levelInfos.Join(scenes, levelInf => levelInf.LevelName, scn => scn.GetClearName(), (levelInf, scn) => new { LevelInfo = levelInf, Scene = scn }).ToList().
                ForEach(elem => {
                    List<int> prevLevelNums = elem.Scene.GetPreviousLevels();
                    elem.LevelInfo.PrevLevels = levelInfos.Where(lvlInfo => prevLevelNums.Contains(lvlInfo.LevelNumber)).ToList();
                });
            return levelInfos;
        }
        private List<Scene> GetScenes()
        {
            List<Scene> scenes = new List<Scene>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(SceneManager.GetSceneByPath(scene.path));
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
}

