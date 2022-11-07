using Assets.Cryo.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionErrorBox : MonoBehaviour
{
    [SerializeField] private Text errorText;
    [SerializeField] private CryoUI cryo;
    void Start()
    {
        
    }

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
}
