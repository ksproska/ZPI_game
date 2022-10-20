using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using LevelUtils;

public class LvlButton : MonoBehaviour

{

    [SerializeField] private Material lineMaterial;

    public Sprite OffSprite;
    public Sprite OnSprite;
    public Button But;
    private List<LvlButton> PrevBut = new List<LvlButton>();

    public bool ActiveStatus = false;
    private bool IsDone = false;
    private LoadSaveHelper.SlotNum ActSlot = LoadSaveHelper.SlotNum.First;

    public Image checkImage;

    private List<LineRenderer> renderersList = new List<LineRenderer>();
    private List<LvlButton> listOfDone = new List<LvlButton>();
    // Start is called before the first frame update
    void Start()
    {
        if(LevelMap.IsLevelDone(this.name, ActSlot))
        {
            IsDone = true;
            ActiveStatus = true;
            But.image.sprite = OnSprite;
            checkImage.gameObject.SetActive(true);

        }


        List<string> prevLevelsNames = LevelMap.GetPrevGameObjectNames(this.name, ActSlot);

        foreach (var name in prevLevelsNames)
        {
            if (LevelMap.IsLevelDone(name, ActSlot))
            {
                ActiveStatus = true;
            }
            PrevBut.Add(GameObject.Find(name).GetComponent<LvlButton>());

        }

        if (!ActiveStatus)
        {
            But.enabled = false;
        }

        foreach (var _ in PrevBut)
        {
            GameObject lineRendererObject =  new GameObject();
            var lineRenderer = lineRendererObject.AddComponent<LineRenderer>();
            lineRendererObject.transform.SetParent(this.transform);
            lineRenderer.material = lineMaterial;
            lineRenderer.positionCount = 2;
            lineRenderer.sortingOrder = 1;
            lineRenderer.startWidth = 0.5f;
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
            lineRenderer.SetPositions(pathPoints);

        }
    }

    
}
