using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LevelUtils;
using CurrentState;
using UnityEngine.Events;


public class MenuSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IMenuActive
{
    [SerializeField] private Text text;
    [SerializeField] private Image background;
    [SerializeField] private Button binButton;
    [SerializeField] private Image icon;
    [SerializeField] private LoadSaveHelper.SlotNum slotNumber;

    [SerializeField] private Sprite enabledSlotSprite;
    [SerializeField] private Sprite disabledSlotSprite;
    [SerializeField] private string enabledSlotName;
    [SerializeField] private string disabledSlotName;

    [NonSerialized] private GoToScene goToScene;
    [NonSerialized] private bool isEnabled;
    [NonSerialized] private bool hasSavedSlots;

    public bool IsEnabled => isEnabled;

    private void Awake()
    {
        var enabledSlots = LoadSaveHelper.Instance.GetOccupiedSlots();
        goToScene = GetComponent<GoToScene>();
        if (enabledSlots.Contains(slotNumber))
        {
            HasSavedGame(true);
        }
        else
        {
            HasSavedGame(false);
        }
    }
    private void OnEnable()
    {
        text.color = Color.black;
        background.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.white;
        background.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.black;
        background.color = Color.white;
    }

    public void SetEnabled(bool isActive)
    {
        isEnabled = isActive;
        GetComponent<Button>().interactable = isActive;
        binButton.interactable = isActive;
    }

    public void HasSavedGame(bool isSaved)
    {
        hasSavedSlots = isSaved;
        binButton.gameObject.SetActive(isSaved);
        if(isSaved)
        {
            icon.sprite = enabledSlotSprite;
            text.text = enabledSlotName;
        }
        else
        {
            icon.sprite = disabledSlotSprite;
            text.text = disabledSlotName;
        }
    }

    public void SetCurrentSlot()
    {
        CurrentGameState.Instance.CurrentSlot = slotNumber;
    }

    public void DeleteSave()
    {
        LoadSaveHelper.Instance.EraseASlot(slotNumber);
        HasSavedGame(false);
    }

    public void GoToScene()
    {
        if (hasSavedSlots)
        {
            goToScene.FadeOutScene();
            return;
        }

        goToScene.scene = "map_0_StoryBeginning";
        var currentState = FindObjectOfType<CurrentGameState>();
        currentState.CurrentLevelName = goToScene.scene;
        goToScene.FadeOutScene();
    }
}
