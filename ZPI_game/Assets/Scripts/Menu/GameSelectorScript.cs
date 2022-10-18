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
    [SerializeField] Image image1;
    [SerializeField] Image image2;
    [SerializeField] Image image3;
    [SerializeField] Text text1;
    [SerializeField] Text text2;
    [SerializeField] Text text3;
    private List<LoadSaveHelper.SlotNum> enabledSlots;

    private void Start()
    {
        enabledSlots = LoadSaveHelper.GetOccupiedSlots();
        //image1 = slot1.gameObject.GetComponentInChildren<Image>();
        //image2 = slot2.GetComponentInChildren<Image>();
        //image3 = slot3.GetComponentInChildren<Image>();
        //text1 = slot1.GetComponentInChildren<Text>();
        //text2 = slot2.GetComponentInChildren<Text>();
        //text3 = slot3.GetComponentInChildren<Text>();
        SetupGameSlots();
    }

    private void SetupGameSlots()
    {
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

    private void OnEnable()
    {
        
    }
}
