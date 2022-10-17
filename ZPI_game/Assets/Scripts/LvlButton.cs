using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LvlButton : MonoBehaviour

{

    [SerializeField] private Material lineMaterial;

    public Sprite OffSprite;
    public Sprite OnSprite;
    public Button But;
    public List<LvlButton> PrevBut;

    public bool ActiveStatus = false;
    public bool IsDone = false;

    private LineRenderer _lineRenderer;
    public Image checkImage;

    private List<LvlButton> listOfDone = new List<LvlButton>();
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.material = lineMaterial;
        _lineRenderer.positionCount = PrevBut.Count + 1;
        listOfDone.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

        if (PrevBut != null)
        {
            foreach(LvlButton but in PrevBut) {
                if(but != null && but.IsDone){
                    ActiveStatus = true;
                    But.image.sprite = OnSprite;
                    if(!listOfDone.Contains(but)){
                        listOfDone.Add(but);
                    }
                    DrawLine();
                }
            }
        }
    }

    public void DrawLine()
    {
        _lineRenderer.positionCount = listOfDone.Count;
        _lineRenderer.SetPositions(listOfDone.Select(level => level.transform.position).ToArray());

        
    }

    public void ChangeImage()
    {
        if (ActiveStatus)
        {
            IsDone = true;
            checkImage.gameObject.SetActive(true);
        }
    }
}
