
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;
using DeveloperUtils;
using UnityEditor;
using System.IO;

namespace Webserver
{
    public class Tests : MonoBehaviour
    {
        async void Start()
        {
            //List<Map> maps = await MapSynchro.GetMaps();
            //JsonSerializer.Serialize(maps, new JsonSerializerOptions { WriteIndented = true }).Debug();

            //(var res, string serverResp) = await Auth.CreateNewUser(new User("janko-z-bogdanca@wp.pl", "Kozaczek42", "haslokajdsajjcjk213"));
            //serverResp.Debug();
            //res.ToString().Debug();

            (var res, string serverResp) = await Auth.AuthenticateUser(new User("janko-z-bogdanca@wp.pl", "haslokajdsajjcjk213"));
            serverResp.Debug();
            res.ToString().Debug();
            GetScenes().DebugLoop();
        }

        private List<string> GetScenes()
        {
            List<string> scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(Path.GetFileNameWithoutExtension(scene.path));
                }
            }
            return scenes;
        }


    }
}

