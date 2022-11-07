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
        errorText.text = Resources.Load<TextAsset>("MenuMessages/internet_connection_error").text;
        cryo.SetBothEyesTypes(Assets.Cryo.Script.EyeType.Sad);
        cryo.SetMouthType(Assets.Cryo.Script.MouthType.Confused);
    }
}
