using Assets.Cryo.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoBehaviour : MonoBehaviour
{
    [SerializeField] SpriteRenderer leftEye;
    [SerializeField] SpriteRenderer rightEye;
    [SerializeField] SpriteRenderer mouth;

    [SerializeField] List<Sprite> eyeTypes;
    [SerializeField] List<Sprite> mouthTypes;

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
                SetBothEyes(EyeType.EyeSmall);
                index += 1;
            }
            else if (index == 1)
            {
                SetBothEyes(EyeType.Wink);
                index += 1;
            }
            else
            {
                SetBothEyes(EyeType.EyeBig);
                index = 0;
            }
        }
    }

    public Sprite GetEyeGraphic(EyeType eyeType)
    {
        switch(eyeType)
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
            case EyeType.Happy | EyeType.Sad:
                return eyeTypes[5];
            default:
                return null;
        }
    }

    public void SetBothEyes(EyeType eyeType)
    {
        SetRightEye(eyeType);
        SetLeftEye(eyeType);
    }

    public void SetRightEye(EyeType eyeType)
    {
        Sprite sprite = GetEyeGraphic(eyeType);
        rightEye.sprite = sprite;
    }

    public void SetLeftEye(EyeType eyeType)
    {
        Sprite sprite = GetEyeGraphic(eyeType);
        leftEye.sprite = sprite;
        leftEye.flipX = false;
        if(eyeType == EyeType.Wink || eyeType == EyeType.Angry)
        {
            leftEye.flipX = true;
        }
    }
}
