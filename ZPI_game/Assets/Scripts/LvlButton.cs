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

    private List<LineRenderer> renderersList = new List<LineRenderer>();
    private List<LvlButton> listOfDone = new List<LvlButton>();
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.material = lineMaterial;
        _lineRenderer.positionCount = PrevBut.Count + 1;

        foreach(var _ in PrevBut)
        {
            GameObject lineRendererObject =  new GameObject();
            var lineRenderer = lineRendererObject.AddComponent<LineRenderer>();
            lineRendererObject.transform.SetParent(this.transform);
            lineRenderer.material = lineMaterial;
            lineRenderer.positionCount = 2;
            lineRenderer.sortingOrder = 1;
            renderersList.Add(lineRenderer);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (PrevBut != null)
        {
            foreach (var item in PrevBut)
            {
                if(item != null && item.IsDone){
                    ActiveStatus = true;
                    But.image.sprite = OnSprite;
                    if(!listOfDone.Contains(item)){
                        listOfDone.Add(item);
                    }
                    DrawLine();
                }
            }
        }
    }

    public void DrawLine()
    {
        foreach (var DoneLevel in listOfDone.Select((value, i) => (value, i)))
        {
            LineRenderer lineRenderer = renderersList[DoneLevel.i];
            Vector3[] pathPoints = { this.transform.position, DoneLevel.value.transform.position };
            Debug.Log(this.transform.position);
            lineRenderer.SetPositions(pathPoints);

        }
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
