using CurrentState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeInfoFrame : MonoBehaviour
{
    [SerializeField] private Text challengeName;
    [SerializeField] private Text bestScore;
    [SerializeField] private Text leaderboard;

    private void Awake()
    {
        CurrentGameState.Instance.CurrentLevelName = "Challenges";
    }
    public void ShowInfoFrame(string challengeName, int bestScore, Image image)
    {
        this.challengeName.text = challengeName;
        this.bestScore.text = $"Best score: {bestScore}";
        leaderboard.text = ""; // load from server
        
        gameObject.SetActive(true);
    }
}
