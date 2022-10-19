using LevelUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectorScript : MonoBehaviour
{
    [SerializeField] GameObject slot1;
    [SerializeField] GameObject slot2;
    [SerializeField] GameObject slot3;
    [SerializeField] Sprite slotNotUsedImage;
    [SerializeField] Sprite slot1Image;
    [SerializeField] Sprite slot2Image;
    [SerializeField] Sprite slot3Image;
    [NonSerialized] Image image1;
    [NonSerialized] Image image2;
    [NonSerialized] Image image3;
    [NonSerialized] Text text1;
    [NonSerialized] Text text2;
    [NonSerialized] Text text3;
    private List<LoadSaveHelper.SlotNum> enabledSlots;

    private void Start()
    {
        image1 = slot1.GetComponentsInChildren<Image>()[1];
        image2 = slot2.GetComponentsInChildren<Image>()[1];
        image3 = slot3.GetComponentsInChildren<Image>()[1];
        text1 = slot1.GetComponentInChildren<Text>();
        text2 = slot2.GetComponentInChildren<Text>();
        text3 = slot3.GetComponentInChildren<Text>();
        SetupGameSlots();
    }

    private void SetupGameSlots()
    {
        enabledSlots = LoadSaveHelper.GetOccupiedSlots();
        if (enabledSlots.Contains(LoadSaveHelper.SlotNum.First))
        {
            image1.sprite = slot1Image;
            text1.text = "GAME 1";
        }
        else
        {
            image1.sprite = slotNotUsedImage;
            text1.text = "EMPTY";
        }

        if (enabledSlots.Contains(LoadSaveHelper.SlotNum.Second))
        {
            image2.sprite = slot2Image;
            text2.text = "GAME 2";
        }
        else
        {
            image2.sprite = slotNotUsedImage;
            text2.text = "EMPTY";
        }

        if (enabledSlots.Contains(LoadSaveHelper.SlotNum.Third))
        {
            image3.sprite = slot3Image;
            text3.text = "GAME 3";
        }
        else
        {
            image3.sprite = slotNotUsedImage;
            text3.text = "EMPTY";
        }
    }

    private LoadSaveHelper.SlotNum GetSlotNumber(int number)
    {
        LoadSaveHelper.SlotNum slotNum;
        switch (number)
        {
            case 1:
                slotNum = LoadSaveHelper.SlotNum.First;
                break;
            case 2:
                slotNum = LoadSaveHelper.SlotNum.Second;
                break;
            case 3:
                slotNum = LoadSaveHelper.SlotNum.Third;
                break;
            default:
                slotNum = LoadSaveHelper.SlotNum.First;
                break;
        }
        return slotNum;
    }

    public void DeleteSave(int saveNumber)
    {
        Debug.Log("Delete");
        var slotNum = GetSlotNumber(saveNumber);
        LoadSaveHelper.EraseASlot(slotNum);
        SetupGameSlots();
    }

    //public void CreateSave(int saveNumber)
    //{
    //    var slotNum = GetSlotNumber(saveNumber);
    //}
}
