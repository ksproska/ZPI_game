using System;
using Assets.Cryo.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionErrorBox : MonoBehaviour
{
    [SerializeField] private Text errorText;
    [SerializeField] private CryoUI cryo;

    public bool IsLoginSuccessful { get; set; } = false;

    public void SetErrorText(string text)
    {
        errorText.text = text;
    }

    public void SetCryoEyeType(EyeType eyeType)
    {
        cryo.SetBothEyesTypes(eyeType);
    }

    public void SetCryoMouthType(MouthType mouthType)
    {
        cryo.SetMouthType(mouthType);
    }

    public void BackButtonAction()
    {
        if (!IsLoginSuccessful) return;
        IsLoginSuccessful = false;
        var menu = FindObjectOfType<MainMenu>();
        menu.SpeedUp();
        menu.SlideOut(0);
    }
}
