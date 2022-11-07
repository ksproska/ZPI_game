
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using UnityEngine;

namespace Maps
{
    public class LocalMaps : MonoBehaviour
    {
        public const string JSON_FILE_NAME = "/Maps/local_maps.json";
        public const string JSON_FILE_NAME_TESTS = "Assets\\Maps\\Tests\\local_maps.json";
        public const int DEF_LOCAL_MAPS_NUMBER = 5;
        public static LocalMaps Instance { get; set; }
        public List<Map> Maps { get { return _maps; } }
        public Map GetMapById(int id)
        {
            return _maps.Find(map => map.MapId == id);
        }
        private List<Map> _maps;
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
            _maps = GetLocalMaps(Application.persistentDataPath + JSON_FILE_NAME);
        }
        public void DeleteMap(int mapId)
        {
            if(mapId < 1)
            {
                throw new ArgumentOutOfRangeException($"{mapId} must be greater than 0");
            }
            else if (mapId < DEF_LOCAL_MAPS_NUMBER)
            {
                throw new ArgumentException($"Maps with id in the range <0, {DEF_LOCAL_MAPS_NUMBER}> are default maps, so they cannot be deleted");
            }
            _maps = _maps.Where(map => map.MapId != mapId).ToList();
            SaveCurrState(Application.persistentDataPath + JSON_FILE_NAME);
        }
        public void DeleteMapTest(int mapId)
        {
            _maps = _maps.Where(map => map.MapId != mapId).ToList();
            SaveCurrState(JSON_FILE_NAME_TESTS);
        }
        public void SaveMap(Map map)
        {
            map.MapId = _maps.OrderByDescending(map => map.MapId).First().MapId + 1;
            _maps.Add(map);
            SaveCurrState(Application.persistentDataPath + JSON_FILE_NAME);
        }
        public void SaveMapTests(Map map)
        {
            _maps.Add(map);
            SaveCurrState(JSON_FILE_NAME_TESTS);
        }
        private void SaveCurrState(string filePath)
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringMaps = JsonSerializer.Serialize(_maps, options);
            File.WriteAllText(filePath, jsonStringMaps);
        }
        private void CopyDefMaps(string filePath)
        {
            string defMapsSerialized = Resources.Load<TextAsset>("Maps/local_maps").text;
            List<Map> defMaps = JsonSerializer.Deserialize<List<Map>>(defMapsSerialized);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringMaps = JsonSerializer.Serialize(defMaps, options);
            File.WriteAllText(filePath, jsonStringMaps);
        }
        private List<Map> GetLocalMaps(string filePath)
        {
            new FileInfo(filePath).Directory.Create();

            if (!File.Exists(filePath))
                CopyDefMaps(filePath);

            string jsonFile = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Map>>(jsonFile);
        }
        
    }

}
