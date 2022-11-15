using CurrentState;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    private void Awake()
    {
        CurrentGameState.Instance.CurrentLevelName = "Challenges";
    }
    public void ShowInfoFrame(InfoFrameBundle bundle)
    {
        leaderboard.gameObject.SetActive(true);
        leaderboardTitle.gameObject.SetActive(true);

        challengeName.text = bundle.ChallengeName;

        var s1 = bundle.Slot1BestScore >= 0 ? bundle.Slot1BestScore.ToString("0.00") : " - ";
        var s2 = bundle.Slot2BestScore >= 0 ? bundle.Slot2BestScore.ToString("0.00") : " - ";
        var s3 = bundle.Slot3BestScore >= 0 ? bundle.Slot3BestScore.ToString("0.00") : " - ";

        slot1BestScore.text = $"<color='#cc0000ff'>Game 1</color>\n{s1}";
        slot2BestScore.text = $"<color='#3300ffff'>Game 2</color>\n{s2}";
        slot3BestScore.text = $"<color='#ff9900ff'>Game 3</color>\n{s3}";

        var accountBest = "";
        if(CurrentGameState.Instance.CurrentUserId == -1)
        {
            accountBest = "not logged in";
            leaderboard.gameObject.SetActive(false);
            leaderboardTitle.gameObject.SetActive(false);
        }
        else if(bundle.AccountBestScore >= 0)
        {
            accountBest = bundle.AccountBestScore.ToString("0.00");
        }
        else
        {
            accountBest = " - ";
        }
        bestScoreForAccount.text = $"Best score: {accountBest}";

        leaderboard.text = TopListToString(bundle.TopScores);
        
        gameObject.SetActive(true);
    }

    private string TopListToString(List<(string, float)> topList)
    {
        var ret = "";
        topList
            .Select((value, index) => (index, value))
            .ToList()
            .ForEach(elem => ret += $"{elem.index + 1}. {elem.value.Item1}\n");
        return ret;
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
    public List<(string, float)> TopScores { get; set; }
    public float AccountBestScore { get; set; }
}
