using DeveloperUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoggedStatus : MonoBehaviour
{
    [SerializeField] private Text text;
    private void OnEnable()
    {
        if(IsLoggedIn())
        {
            CurrentState.CurrentGameState.Instance.CurrentUserNickname.Debug();
            text.text = $"Logged in as {CurrentState.CurrentGameState.Instance.CurrentUserNickname}";
        }
        else
        {
            text.text = "Not logged in";
        }
    }

    private bool IsLoggedIn()
    {
        return CurrentState.CurrentGameState.Instance.CurrentUserId != -1;
    }
}
