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
    
    EyeType currentLeftEyeType;
    EyeType currentRightEyeType;

    //float seconds = 0;
    //float delta = 2;
    //int index = 0;

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
            case EyeType.Happy: case EyeType.Sad:
                return eyeTypes[5];
            default:
                return null;
        }
    }

    public Sprite GetMouthGraphic(MouthType mouthType)
    {
        switch(mouthType)
        {
            case MouthType.Angry:
                return mouthTypes[0];
            case MouthType.Confused:
                return mouthTypes[1];
            case MouthType.Crying:
                return mouthTypes[2];
            case MouthType.Smile: case MouthType.Sad:
                return mouthTypes[3];
            default:
                return null;
        }
    }

    public void SetMouthType(MouthType mouthType)
    {
        Sprite sprite = GetMouthGraphic(mouthType);
        mouth.flipY = false;
        if(MouthTypeHandler.RequireFlip(mouthType))
        {
            mouth.flipY = true;
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
        rightEye.flipX = false;
        rightEye.flipY = false;
        Sprite sprite = GetEyeGraphic(eyeType);
        rightEye.sprite = sprite;
        currentRightEyeType = eyeType;
    }

    public void SetLeftEyeType(EyeType eyeType)
    {
        leftEye.flipX = false;
        leftEye.flipY = false;
        Sprite sprite = GetEyeGraphic(eyeType);
        leftEye.sprite = sprite;
        currentLeftEyeType = eyeType;
        leftEye.flipX = false;
        if(EyeTypeHandler.IsSimmetric(eyeType))
        {
            leftEye.flipX = true;
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

    private void AssignEyeDirection(SpriteRenderer eye, EyeDirection direction)
    {
        switch(direction)
        {
            case EyeDirection.UpLeft:
                eye.flipX = false;
                eye.flipY = false;
                return;
            case EyeDirection.UpRight:
                eye.flipX = true;
                eye.flipY = false;
                return;
            case EyeDirection.DownLeft:
                eye.flipX = false;
                eye.flipY = true;
                return;
            case EyeDirection.DownRight:
                eye.flipX = true;
                eye.flipY = true;
                return;
            default:
                return;
        }
    }
}
