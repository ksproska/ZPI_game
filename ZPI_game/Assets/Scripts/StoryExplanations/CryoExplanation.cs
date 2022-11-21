using Assets.Cryo.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CryoExplanation : MonoBehaviour
{
    [SerializeField] private CryoUI cryo;
    [SerializeField] private EyeType leftEye;
    [SerializeField] private EyeType rightEye;
    [SerializeField] private MouthType mouthType;
    [SerializeField] private Text explanation;
    [SerializeField] private string textResource;

    private void Start()
    {
        cryo.SetLeftEyeType(leftEye);
        cryo.SetRightEyeType(rightEye);
        cryo.SetMouthType(mouthType);

        explanation.text = Resources.Load<TextAsset>($"CryoExplanations/{textResource}").text;
    }
}
