
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
            _maps = GetLocalMaps();
        }
        private List<Map> GetLocalMaps()
        {
            string defMapsSerialized = Resources.Load<TextAsset>("Maps/local_maps").text;
            return JsonSerializer.Deserialize<List<Map>>(defMapsSerialized);
        }

        //public void SaveMap(Map map)
        //{
        //    map.MapId = _maps.OrderByDescending(map => map.MapId).First().MapId + 1;
        //    _maps.Add(map);
        //    SaveCurrState(Application.persistentDataPath + JSON_FILE_NAME);
        //}

        //private void SaveCurrState(string filePath)
        //{
        //    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        //    string jsonStringMaps = JsonSerializer.Serialize(_maps, options);
        //    File.WriteAllText(filePath, jsonStringMaps);
        //}

    }

}
