using Assets.Cryo.Script;
using CurrentState;
using LevelUtils;
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
    [SerializeField] private float secondsTillBlink;
    [SerializeField] private Image stopper;

    System.Random rnd = new();
    private float lastUpdate = 0;

    private void Start()
    {
        cryo.SetLeftEyeType(leftEye);
        cryo.SetRightEyeType(rightEye);
        cryo.SetMouthType(mouthType);

        explanation.text = Resources.Load<TextAsset>($"CryoExplanations/{textResource}").text;

        SetFrameActive(LevelMap.Instance.IsLevelDone(CurrentGameState.Instance.CurrentLevelName,
            CurrentGameState.Instance.CurrentSlot));
    }

    private void FixedUpdate()
    {
        lastUpdate += Time.deltaTime;
        if (lastUpdate > secondsTillBlink)
        {
            secondsTillBlink = (float)(rnd.NextDouble() * 10 + 5);
            lastUpdate = 0;
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        cryo.SetBothEyesTypes(EyeType.Closed);
        yield return new WaitForSeconds(0.5f);
        cryo.SetLeftEyeType(leftEye);
        cryo.SetRightEyeType(rightEye);
    }

    public void SetFrameActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        if(stopper != null)
        {
            stopper.gameObject.SetActive(isActive);
        }
    }
}
