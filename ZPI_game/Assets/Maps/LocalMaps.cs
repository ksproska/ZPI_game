
using System.Collections.Generic;
using System.IO;
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
            _maps = GetLocalMaps(Application.persistentDataPath + JSON_FILE_NAME);
        }
        private void CreateDefMaps(string filePath)
        {
            var defMaps = new List<Map>() { 
                new Map(new List<Point>() { new Point(10.25f, 0.2f), new Point(10.25f, 0.2f), new Point(10.25f, 0.2f), new Point(10.25f, 0.2f)}, 1),
                new Map(new List<Point>() { new Point(10.25f, 0.2f), new Point(10.25f, 0.2f), new Point(10.25f, 0.2f), new Point(10.25f, 0.2f)}, 2),
                new Map(new List<Point>() { new Point(10.25f, 0.2f), new Point(10.25f, 0.2f), new Point(10.25f, 0.2f), new Point(10.25f, 0.2f)}, 3)
            };
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringMaps = JsonSerializer.Serialize(defMaps, options);
            File.WriteAllText(filePath, jsonStringMaps);
        }
        private List<Map> GetLocalMaps(string filePath)
        {
            new FileInfo(filePath).Directory.Create();

            if (!File.Exists(filePath))
                CreateDefMaps(filePath);

            string jsonFile = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Map>>(jsonFile);
        }
        
    }

}
