using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using UnityEngine.Networking;
using Maps;

namespace Webserver
{
    public static class MapSynchro
    {
        public static async Task<UnityWebRequest.Result> CreateNewMap(Map map)
        {
            using UnityWebRequest wr = new UnityWebRequest("http://localhost:5000/api/map", "POST");
            wr.SetRequestHeader("Content-Type", "application/json");
            byte[] rawMapSerialized = Encoding.UTF8.GetBytes(MapUtils.MapToJson(map));
            wr.uploadHandler = new UploadHandlerRaw(rawMapSerialized);
            
            var asyncOperation = wr.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            return wr.result;
        }
        public static async Task<List<int>> GetMapsId()
        {
            using UnityWebRequest wr = new UnityWebRequest("http://localhost:5000/api/map_ids", "GET");
            wr.SetRequestHeader("Content-Type", "application/json");
            wr.downloadHandler = new DownloadHandlerBuffer();

            var asyncOperation = wr.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            if (wr.result == UnityWebRequest.Result.Success)
            {
                string jsonResp = Encoding.UTF8.GetString(wr.downloadHandler.data);
                return JsonSerializer.Deserialize<List<int>>(jsonResp);
            }
            else
            {
                return new List<int>();
            }
        }
        public static async Task<List<Map>> GetMaps()
        {
            using UnityWebRequest wr = new UnityWebRequest("http://localhost:5000/api/maps", "GET");
            wr.downloadHandler = new DownloadHandlerBuffer();

            var asyncOperation = wr.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            if (wr.result == UnityWebRequest.Result.Success)
            {
                string jsonResp = Encoding.UTF8.GetString(wr.downloadHandler.data);
                return JsonSerializer.Deserialize<List<Map>>(jsonResp);
            }
            else
            {
                return new List<Map>();
            }
        }
        public static async Task<Map> GetMap(int mapId)
        {
            using UnityWebRequest wr = new UnityWebRequest($"http://localhost:5000/api/map/{mapId}", "GET");
            wr.downloadHandler = new DownloadHandlerBuffer();

            var asyncOperation = wr.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            if (wr.result == UnityWebRequest.Result.Success)
            {
                string jsonResp = Encoding.UTF8.GetString(wr.downloadHandler.data);
                return JsonSerializer.Deserialize<Map>(jsonResp);
            }
            else
            {
                return null;
            }
        }
        public static async Task<List<Point>> GetMapsPoints(int mapId)
        {
            using UnityWebRequest wr = new UnityWebRequest($"http://localhost:5000/api/points/{mapId}", "GET");
            wr.SetRequestHeader("Content-Type", "application/json");
            wr.downloadHandler = new DownloadHandlerBuffer();

            var asyncOperation = wr.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            if (wr.result == UnityWebRequest.Result.Success)
            {
                string jsonResp = Encoding.UTF8.GetString(wr.downloadHandler.data);
                return JsonSerializer.Deserialize<List<Point>>(jsonResp);
            }
            else
            {
                return new List<Point>();
            }
        }
        public static async Task<(UnityWebRequest.Result, string)> CreateNewUsrMap(Map map)
        {
            using UnityWebRequest wr = new UnityWebRequest($"http://localhost:5000/api/user/{map.CreatorId}/map", "POST");
            wr.SetRequestHeader("Content-Type", "application/json");
            wr.downloadHandler = new DownloadHandlerBuffer();
            byte[] rawMapSerialized = Encoding.UTF8.GetBytes(map.ToJson());
            wr.uploadHandler = new UploadHandlerRaw(rawMapSerialized);

            var asyncOperation = wr.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            string srvResp = wr.downloadHandler.data != null ? Encoding.UTF8.GetString(wr.downloadHandler.data) : "";
            return (wr.result, srvResp);
        }
        public static async Task<(UnityWebRequest.Result, List<Map>)> GetUserMaps(int userId)
        {
            using UnityWebRequest wr = new UnityWebRequest($"http://localhost:5000/api/user/{userId}/maps", "GET");
            wr.downloadHandler = new DownloadHandlerBuffer();

            var asyncOperation = wr.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            if (wr.result == UnityWebRequest.Result.Success)
            {
                string jsonResp = Encoding.UTF8.GetString(wr.downloadHandler.data);
                JsonSerializerOptions options = new JsonSerializerOptions();
                return (wr.result, JsonSerializer.Deserialize<List<Map>>(jsonResp));
            }
            else
            {
                return (wr.result, new List<Map>());
            }
        }
        
    }

}

