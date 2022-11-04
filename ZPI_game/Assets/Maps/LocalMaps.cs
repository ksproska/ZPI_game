
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
        private void CreateDefMaps(string filePath)
        {
            var defMaps = new List<Map>() { 
                new Map(new List<Point>() { new Point(10, 407), new Point(840, 24), new Point(237, 333), new Point(179, 999), new Point(432, 682), new Point(111, 298), new Point(899, 781), new Point(524, 19), new Point(726, 947), new Point(682, 124)}, 1),
                new Map(new List<Point>() { new Point(1001, 263), new Point(484, 944), new Point(906, 102), new Point(306, 762), new Point(432, 432), new Point(461, 520), new Point(578, 22), new Point(197, 466), new Point(211, 300), new Point(558, 814), new Point(146, 206), new Point(752, 309), new Point(904, 26) }, 2),
                new Map(new List<Point>() { new Point(24, 67), new Point(215, 1000), new Point(349, 782), new Point(331, 545), new Point(444, 671), new Point(871, 205), new Point(220, 357), new Point(93, 435), new Point(909, 531), new Point(865, 214), new Point(204, 902), new Point(336, 707), new Point(904, 235), new Point(245, 105), new Point(653, 572), new Point(164, 435), new Point(983, 102)}, 3),
                new Map(new List<Point>() { new Point(288, 689), new Point(490, 358), new Point(593, 934), new Point(128, 465), new Point(885, 780), new Point(871, 205), new Point(726, 604), new Point(260, 403), new Point(873, 488), new Point(865, 214), new Point(178, 759), new Point(584, 518), new Point(907, 696), new Point(15, 423), new Point(61, 460), new Point(836, 236), new Point(798, 232), new Point(42, 140), new Point(333, 983), new Point(566, 413), new Point(834, 513)}, 4),
                new Map(new List<Point>() { new Point(403, 265), new Point(810, 446), new Point(938, 222), new Point(128, 465), new Point(787, 532), new Point(658, 642), new Point(785, 325), new Point(301, 645), new Point(873, 488), new Point(59, 87), new Point(338, 390), new Point(781, 673), new Point(92, 168), new Point(15, 423), new Point(143, 646), new Point(447, 564), new Point(636, 537), new Point(683, 411), new Point(333, 983), new Point(895, 997), new Point(489, 696), new Point(403, 265), new Point(810, 446), new Point(938, 222), new Point(128, 465), new Point(787, 532), new Point(658, 642), new Point(785, 325), new Point(301, 645), new Point(873, 488), new Point(59, 87), new Point(338, 390), new Point(781, 673), new Point(92, 168), new Point(15, 423), new Point(143, 646), new Point(447, 564), new Point(636, 537), new Point(357, 254), new Point(728, 225), new Point(793, 465), new Point(128, 902)}, 5)
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
