using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    private int widthScaler = 1920;
    private int heightScaler = 1080;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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


    
}
