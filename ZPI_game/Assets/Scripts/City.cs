using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    private int widthScaler = 1920;
    private int heightScaler = 1080;

    [SerializeField] private TextMesh text;

    public int cityNumber;
    public (float, float) GetPosition()
    {
        return (gameObject.transform.position.x, gameObject.transform.position.y);
    }

    public float Distance(City other)
    {
        var (otherX, otherY) = other.GetPosition();
        var (thisX, thisY) = GetPosition();
        return Mathf.Sqrt((otherX - thisX) * (otherX - thisX) + (otherY - thisY) * (otherY - thisY));
    }

    public void SetText(string arg)
    {
        text.text = arg;
    }
}
