using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WordType
{
    Basic = 0,
    Function,
    Keyword,
    Argument,
    Decorator,
    Type,
    String,
    DefaultArgument,
}

public class TextColor
{
    public static Color GetTextColor(WordType type)
    {
        switch(type)
        {
            case WordType.Basic:
                return new Color(0.898f, 0.898f, 0.898f);
            case WordType.Function:
                return new Color(0.612f, 0.431f, 0.859f);
            case WordType.Type:
                return new Color(1f, 0f, 127);
            case WordType.Keyword:
                return new Color(0.961f, 0.718f, 0f);
            case WordType.Argument:
                return new Color(0.165f, 0.58f, 0.949f);
            case WordType.Decorator:
                return new Color(1f, 0.78f, 0.333f);
            case WordType.String:
                return new Color(0.537f, 0.988f, 0f);
            case WordType.DefaultArgument:
                return new Color(1f, 0.569f, 0f);
            default:
                return new Color(0.898f, 0.898f, 0.898f);
        }
    }
}
public class StaticText : MonoBehaviour
{
    [SerializeField] public WordType wordType;
    [SerializeField] public bool showParenthesis;

    [NonSerialized] Text mainText;
    [SerializeField] Text openParenthesis;
    [SerializeField] Text closrParenthesis;
    void Start()
    {
        mainText = GetComponent<Text>();
        mainText.color = TextColor.GetTextColor(wordType);
        if (openParenthesis != null && closrParenthesis != null)
        {
            openParenthesis.gameObject.SetActive(false);
            closrParenthesis.gameObject.SetActive(false);
            if (showParenthesis)
            {
                openParenthesis.gameObject.SetActive(true);
                closrParenthesis.gameObject.SetActive(true);
            }
        }
    }

    
    void Update()
    {
        
    }
}
