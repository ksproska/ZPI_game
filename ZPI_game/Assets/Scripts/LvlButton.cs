using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlButton : MonoBehaviour

{

    [SerializeField] private Material lineMaterial;

    public Sprite OffSprite;
    public Sprite OnSprite;
    public Button But;
    public LvlButton PrevBut;

    public bool ActiveStatus = false;
    public bool IsDone = false;

    private LineRenderer _lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.material = lineMaterial;
    }

    // Update is called once per frame
    void Update()
    {

        if (PrevBut != null && PrevBut.IsDone)
        {
            ActiveStatus = true;
            this.DrawLine();
        }
        else if(PrevBut != null)
        {
            _lineRenderer.positionCount = 0;
            ActiveStatus = false;
            But.image.sprite = OffSprite;
            IsDone = false;
        }


    }

    public void DrawLine()
    {
        Vector3[] pathPoints = { this.transform.position, PrevBut.transform.position };
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPositions(pathPoints);

        
    }

    public void ChangeImage()
    {
        if (But.image.sprite == OffSprite && ActiveStatus)
        {
            But.image.sprite = OnSprite;
            IsDone = true;
        }
        else if(PrevBut != null)
        {
            But.image.sprite = OffSprite;
            ActiveStatus = false;
            IsDone = false;

        }
    }
}
