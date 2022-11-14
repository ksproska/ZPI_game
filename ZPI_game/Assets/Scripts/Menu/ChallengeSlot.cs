using DeveloperUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChallengeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image background;
    [SerializeField] Text challengeName;
    [SerializeField] Text bestScore;

    private void OnEnable()
    {
        challengeName.color = Color.black;
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
        "Click".Debug();
    }
}
