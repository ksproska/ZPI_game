using System;
using DeveloperUtils;
using System.Collections;
using System.Collections.Generic;
using CurrentState;
using LevelUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChallengeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image background;
    [SerializeField] Text challengeName;
    [SerializeField] Text bestScore;
    [SerializeField] ChallengeInfoFrame infoFrame;
    [SerializeField] string clearName;
    [SerializeField] int challengeId;
    [SerializeField] string mapPreviewAssetName;

    private void Start()
    {
        var bestForSlot = LoadSaveHelper.Instance.GetSlot(CurrentState.CurrentGameState.Instance.CurrentSlot)
            .BestScores[challengeId];
        if (bestForSlot >= 0)
        {
            bestScore.text = $"Best score: {bestForSlot:0.00}";
        }
        else
        {
            bestScore.text = "Best score: - ";
        }
    }

    private void OnEnable()
    {
        challengeName.color = Color.black;
        challengeName.text = clearName;
        bestScore.color = Color.black;
        background.color = Color.white;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        challengeName.color = Color.white;
        bestScore.color = Color.white;
        background.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        challengeName.color = Color.black;
        bestScore.color = Color.black;
        background.color = Color.white;
    }

    public void ShowLevelInfoFrame()
    {
        var bundle = new InfoFrameBundle();
        bundle.ChallengeName = clearName;
        bundle.ChallengeID = challengeId;
        
        bundle.Slot1BestScore = LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.First).BestScores[challengeId];
        bundle.Slot2BestScore = LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Second).BestScores[challengeId];
        bundle.Slot3BestScore = LoadSaveHelper.Instance.GetSlot(LoadSaveHelper.SlotNum.Third).BestScores[challengeId];

        infoFrame.ShowInfoFrame(bundle);
    }
}
