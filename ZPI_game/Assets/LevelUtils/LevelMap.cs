
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using UnityEngine;

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
        private List<LevelButtonInfo> LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

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
    }
}

