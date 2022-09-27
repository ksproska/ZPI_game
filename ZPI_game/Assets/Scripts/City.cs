using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    private int widthScaler = 1920;
    private int heightScaler = 1080;

    [SerializeField] private TextMesh text;

    public int cityNumber;
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

    public void SetText(string arg)
    {
        text.text = arg;
    }

    //public void DrawLine(City other)
    //{
    //    var lineRenderer = GetComponent<LineRenderer>();
    //    lineRenderer.SetPosition(0, gameObject.transform.position);
    //    lineRenderer.SetPosition(1, other.gameObject.transform.position);
    //}


    
}
