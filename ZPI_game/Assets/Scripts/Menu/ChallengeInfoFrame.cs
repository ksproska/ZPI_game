using System;
using CurrentState;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperUtils;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChallengeInfoFrame : MonoBehaviour
{
    [SerializeField] private Text challengeName;
    [SerializeField] private Text slot1BestScore;
    [SerializeField] private Text slot2BestScore;
    [SerializeField] private Text slot3BestScore;
    [SerializeField] private Text bestScoreForAccount;
    [SerializeField] private Text leaderboard;
    [SerializeField] private Text leaderboardTitle;
    [SerializeField] private Image mapPreview;

    private void Awake()
    {
        CurrentGameState.Instance.CurrentLevelName = "Challenges";
    }

    private void OnEnable()
    {
        if (CurrentGameState.Instance.CurrentUserId != -1)
        {
            bestScoreForAccount.text = "Best score: loading...";
            leaderboard.text = "Loading...";
            return;
        }
        leaderboard.gameObject.SetActive(false);
        leaderboardTitle.gameObject.SetActive(false);
    }

    public async void ShowInfoFrame(InfoFrameBundle bundle)
    {
        leaderboard.gameObject.SetActive(true);
        leaderboardTitle.gameObject.SetActive(true);

        CurrentGameState.Instance.CurrentMapId = bundle.ChallengeID;

        mapPreview.sprite = Resources.Load<Sprite>($"Challenges/challenge{bundle.ChallengeID}");

        challengeName.text = bundle.ChallengeName;

        var s1 = bundle.Slot1BestScore >= 0 ? bundle.Slot1BestScore.ToString("0.00") : " - ";
        var s2 = bundle.Slot2BestScore >= 0 ? bundle.Slot2BestScore.ToString("0.00") : " - ";
        var s3 = bundle.Slot3BestScore >= 0 ? bundle.Slot3BestScore.ToString("0.00") : " - ";

        slot1BestScore.text = $"<color='#cc0000ff'>Game 1</color>\n{s1}";
        slot2BestScore.text = $"<color='#3300ffff'>Game 2</color>\n{s2}";
        slot3BestScore.text = $"<color='#ff9900ff'>Game 3</color>\n{s3}";
        gameObject.SetActive(true);
        var (success, score, topList) = await LoadDataFromServer(bundle.ChallengeID);
        SetupAccountBestScoreInfo(success, score);
        SetupTopFivePlayersInfo(success, topList);
    }

    private async Task<(bool, float, string)> LoadDataFromServer(int challengeID)
    {
        if(CurrentGameState.Instance.CurrentUserId == -1)
            return (false, -1, null);
        float bestScore = -1;
        string topList;
        
        var (unityTopFiveResult, bestScores) = await Webserver.ScoreSynchro.GetTopFiveBestScores(challengeID);
        switch (unityTopFiveResult)
        {
            case UnityWebRequest.Result.Success:
                topList = TopListToString(bestScores);
                break;
            default:
                return (false, -1, "");
        }

        var (unityBestScoreResult, best) = await Webserver.ScoreSynchro.GetUsrBestScore(CurrentGameState.Instance.CurrentUserId, challengeID);
        switch (unityBestScoreResult)
        {
            case UnityWebRequest.Result.Success:
                bestScore = best;
                break; 
            case UnityWebRequest.Result.ProtocolError:
                return (true, -1, topList);
            default:
                return (false, -1, "");
        }

        return (true, bestScore, topList);
    }

    private string TopListToString(List<(string, float)> topList)
    {
        var ret = "";
        topList
            .Select((value, index) => (index, value))
            .ToList()
            .ForEach(elem => ret += $"{elem.index + 1}. {elem.value.Item1}:  {elem.value.Item2:0.00}\n");
        return ret;
    }

    private void SetupAccountBestScoreInfo(bool success, float score)
    {
        if (bestScoreForAccount == null) return;
        if (!success)
        {
            bestScoreForAccount.text = "Best score: not logged in";
            return;
        }
        var bestAccountScore = score >= 0 ? score.ToString("0.00") : " - ";
        bestScoreForAccount.text = $"Best score: {bestAccountScore}";
    }

    private void SetupTopFivePlayersInfo(bool success, string topList)
    {
        if (leaderboardTitle == null) return;
        if (!success)
        {
            leaderboard.gameObject.SetActive(false);
            leaderboardTitle.gameObject.SetActive(false);
            return;
        }
        leaderboard.text = topList;
    }
}

public class InfoFrameBundle
{
    public string ChallengeName { get; set; }
    public int ChallengeID { get; set; }
    public Image PreviewImage { get; set; }
    public float Slot1BestScore { get; set; }
    public float Slot2BestScore { get; set; }
    public float Slot3BestScore { get; set; }
}
