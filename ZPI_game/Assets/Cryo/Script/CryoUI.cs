using Assets.Cryo.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CryoUI : MonoBehaviour
{
    [SerializeField] Image leftEye;
    [SerializeField] Image rightEye;
    [SerializeField] Image mouth;
    [SerializeField] GameObject chatPanel;
    [SerializeField] Text chatText;

    [SerializeField] List<Sprite> eyeTypes;
    [SerializeField] List<Sprite> mouthTypes;

    EyeType currentLeftEyeType;
    EyeType currentRightEyeType;

    float seconds = 0;
    float delta = 2;
    int index = 0;

    void Start()
    {

    }

    void Update()
    {
        seconds += Time.deltaTime;
        if (seconds > delta)
        {
            seconds = 0;
            if (index == 0)
            {
                SetBothEyesTypes(EyeType.EyeBig);
                SetBothEyesDirection(EyeDirection.DownRight);
                SetMouthType(MouthType.Confused);
                Say("Hello", 24);
                index += 1;
            }
            else if (index == 1)
            {
                SetBothEyesTypes(EyeType.Angry);
                SetMouthType(MouthType.Angry);
                Say("I am so f****** angry!");
                index += 1;
            }
            else
            {
                SetBothEyesTypes(EyeType.Wink);
                SetMouthType(MouthType.Sad);
                Say("But not to worry! I'm not angry with you...");
                //SetRightEyeDirection(EyeDirection.UpRight);
                index = 0;
            }
        }
    }

    public Sprite GetEyeGraphic(EyeType eyeType)
    {
        switch (eyeType)
        {
            case EyeType.Eye:
                return eyeTypes[0];
            case EyeType.EyeBig:
                return eyeTypes[1];
            case EyeType.EyeSmall:
                return eyeTypes[2];
            case EyeType.Angry:
                return eyeTypes[3];
            case EyeType.Wink:
                return eyeTypes[4];
            case EyeType.Happy:
            case EyeType.Sad:
                return eyeTypes[5];
            default:
                return null;
        }
    }

    public Sprite GetMouthGraphic(MouthType mouthType)
    {
        switch (mouthType)
        {
            case MouthType.Angry:
                return mouthTypes[0];
            case MouthType.Confused:
                return mouthTypes[1];
            case MouthType.Crying:
                return mouthTypes[2];
            case MouthType.Smile:
            case MouthType.Sad:
                return mouthTypes[3];
            default:
                return null;
        }
    }

    private void FlipImageY(in Image image, bool flip)
    {
        var scale = image.transform.localScale;
        if (flip)
        {
            image.transform.localScale = new Vector3(scale.x, -Mathf.Abs(scale.y));
        } 
        else
        {
            image.transform.localScale = new Vector3(scale.x, Mathf.Abs(scale.y));
        }
    }

    private void FlipImageX(in Image image, bool flip)
    {
        var scale = image.transform.localScale;
        if (flip)
        {
            image.transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y);
        }
        else
        {
            image.transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y);
        }
    }

    public void SetMouthType(MouthType mouthType)
    {
        Sprite sprite = GetMouthGraphic(mouthType);
        FlipImageY(mouth, false);
        if (MouthTypeHandler.RequireFlip(mouthType))
        {
            FlipImageY(mouth, true);
        }
        mouth.sprite = sprite;
    }

    public void SetBothEyesTypes(EyeType eyeType)
    {
        SetRightEyeType(eyeType);
        SetLeftEyeType(eyeType);
    }

    public void SetRightEyeType(EyeType eyeType)
    {
        FlipImageX(rightEye, false);
        FlipImageY(rightEye, false);
        Sprite sprite = GetEyeGraphic(eyeType);
        rightEye.sprite = sprite;
        currentRightEyeType = eyeType;
    }

    public void SetLeftEyeType(EyeType eyeType)
    {
        FlipImageX(leftEye, false);
        FlipImageY(leftEye, false);
        Sprite sprite = GetEyeGraphic(eyeType);
        leftEye.sprite = sprite;
        currentLeftEyeType = eyeType;
        FlipImageX(leftEye, false);
        if (EyeTypeHandler.IsSimmetric(eyeType))
        {
            FlipImageX(leftEye, true);
        }
    }

    public void SetBothEyesDirection(EyeDirection direction)
    {
        SetRightEyeDirection(direction);
        SetLeftEyeDirection(direction);
    }

    public void SetRightEyeDirection(EyeDirection direction)
    {
        if (!EyeTypeHandler.IsRotatable(currentRightEyeType)) return;
        AssignEyeDirection(rightEye, direction);
    }

    public void SetLeftEyeDirection(EyeDirection direction)
    {
        if (!EyeTypeHandler.IsRotatable(currentLeftEyeType)) return;
        AssignEyeDirection(leftEye, direction);
    }

    private void AssignEyeDirection(Image eye, EyeDirection direction)
    {
        switch (direction)
        {
            case EyeDirection.UpLeft:
                FlipImageX(eye, false);
                FlipImageY(eye, false);
                return;
            case EyeDirection.UpRight:
                FlipImageX(eye, true);
                FlipImageY(eye, false);
                return;
            case EyeDirection.DownLeft:
                FlipImageX(eye, false);
                FlipImageY(eye, true);
                return;
            case EyeDirection.DownRight:
                FlipImageX(eye, true);
                FlipImageY(eye, true);
                return;
            default:
                return;
        }
    }

    public void ShowDialogBox(bool show)
    {
        chatPanel.SetActive(show);
    }

    public void Say(string text, int fontSize = 15)
    {
        chatText.text = text;
        chatText.fontSize = fontSize;
        ShowDialogBox(true);
    }
}
