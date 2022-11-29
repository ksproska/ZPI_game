
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

            //(var res, string serverResp) = await ScoreSynchro.PutNewScore(new Score(2, 2, 32.2f));
            //serverResp.Debug();
            //res.ToString().Debug();

            //(var res, float bstScr) = await ScoreSynchro.GetUsrBestScore(2, 2);
            //bstScr.Debug();
            //res.ToString().Debug()
            //var res = await MapSynchro.CreateNewMap(new Map(new List<Point>() { new Point(136.3f, 160.2f), new Point(117.5f, 185.5f), new Point(102.1f, 223.4f), new Point(102.1f, 223.4f) }));
            //res.Debug();
        }




    }
}

