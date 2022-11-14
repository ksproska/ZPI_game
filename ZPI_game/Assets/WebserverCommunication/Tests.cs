
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using DeveloperUtils;
using System.IO;
using Maps;


namespace Webserver
{
    public class Tests : MonoBehaviour
    {
        async void Start()
        {
            //List<Map> maps = await MapSynchro.GetMaps();
            //foreach (var map in maps)
            //{
            //    Debug.Log(map.CreatorId);
            //    Debug.Log(map.CreationDate);
            //    Debug.Log(map.Points.Count);
            //}

            //(var res, string serverResp) = await Auth.CreateNewUser(new User("janko-z-bogdanca@wp.pl", "Kozaczek42", "haslokajdsajjcjk213"));
            //serverResp.Debug();
            //res.ToString().Debug();

            //(var res, string serverResp) = await MapSynchro.CreateNewUsrMap(new Map(new List<Point>() { new Point(455, 267), new Point(109, 87), new Point(85, 332) }, creatorId: 2));
            //serverResp.Debug();
            //res.ToString().Debug();

            //(var res, List<Map> userMaps) = await MapSynchro.GetUserMaps(2);
            //foreach (var map in userMaps)
            //{
            //    Debug.Log(map.CreatorId);
            //    Debug.Log(map.CreationDate);
            //    Debug.Log(map.Points.Count);
            //}
            //res.ToString().Debug();
        }




    }
}

