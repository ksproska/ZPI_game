using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Webserver
{
    public static class Auth
    {
        public static async Task<(UnityWebRequest.Result, string)> CreateNewUser(User user)
        {
            using UnityWebRequest wr = new UnityWebRequest("http://localhost:5000/api/user", "POST");
            wr.SetRequestHeader("Content-Type", "application/json");
            byte[] rawUsrSerialized = Encoding.UTF8.GetBytes(user.ToJson());
            wr.uploadHandler = new UploadHandlerRaw(rawUsrSerialized);
            wr.downloadHandler = new DownloadHandlerBuffer();
            
            var asyncOperation = wr.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            string srvResp = Encoding.UTF8.GetString(wr.downloadHandler.data);
            return (wr.result, srvResp);
        }
        public static async Task<(UnityWebRequest.Result, string)> AuthenticateUser(User user)
        {
            using UnityWebRequest wr = new UnityWebRequest("http://localhost:5000/api/auth", "POST");
            wr.SetRequestHeader("Content-Type", "application/json");
            byte[] rawUsrSerialized = Encoding.UTF8.GetBytes(user.ToJson());
            wr.uploadHandler = new UploadHandlerRaw(rawUsrSerialized);
            wr.downloadHandler = new DownloadHandlerBuffer();

            var asyncOperation = wr.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
            string srvResp = Encoding.UTF8.GetString(wr.downloadHandler.data);
            return (wr.result, srvResp);
        }
    }
    
    public class User
    {
        public string Email { get; private set; }
        public string? Nickname { get; private set; }
        public string Password { get; private set; }
        public User(string email, string nickname, string password)
        {
            Email = email;
            Nickname = nickname;
            Password = password;
        }
        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

