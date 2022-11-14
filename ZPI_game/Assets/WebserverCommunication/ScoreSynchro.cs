using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Webserver
{
    public static class ScoreSynchro
    {
        public static async Task<(UnityWebRequest.Result, string)> PutNewScore(Score score)
        {
            using UnityWebRequest wr = new UnityWebRequest($"http://localhost:5000/api/user/{score.UserId}/score", "POST");
            wr.SetRequestHeader("Content-Type", "application/json");
            byte[] rawScoreSerialized = Encoding.UTF8.GetBytes(score.ToJson());
            wr.uploadHandler = new UploadHandlerRaw(rawScoreSerialized);
            wr.downloadHandler = new DownloadHandlerBuffer();

            var asyncOperation = wr.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            string srvResp = wr.downloadHandler.data != null ? Encoding.UTF8.GetString(wr.downloadHandler.data) : "";
            return (wr.result, srvResp);
        }

        public static async Task<(UnityWebRequest.Result, float)> GetUsrBestScore(int userId, int mapId)
        {
            using UnityWebRequest wr = new UnityWebRequest($"http://localhost:5000/api/user/{userId}/score/{mapId}", "GET");
            wr.SetRequestHeader("Content-Type", "application/json");
            wr.downloadHandler = new DownloadHandlerBuffer();

            var asyncOperation = wr.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            float bestScore = wr.downloadHandler.data != null ? float.Parse(Encoding.UTF8.GetString(wr.downloadHandler.data), System.Globalization.CultureInfo.InvariantCulture) : -1;
            return (wr.result, bestScore);
        }
    }

    public class Score
    {
        public int UserId { get; set; }
        public int MapId { get; set; }
        public float BestScore { get; set; }

        public Score(int userId, int mapId, float bestScore)
        {
            UserId = userId;
            MapId = mapId;
            BestScore = bestScore;
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

