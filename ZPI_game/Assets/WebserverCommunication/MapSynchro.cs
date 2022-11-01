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
                return null;
            }
        }
    }
}

