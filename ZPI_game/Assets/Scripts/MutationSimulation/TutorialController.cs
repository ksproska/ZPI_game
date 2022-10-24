using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{

    public DropHandler dropHandler;
    public GameObject tutorialButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TutorialButtonActivation()
    {
        if (!dropHandler.AreAllCorrect())
        {
            tutorialButton.SetActive(true);
        }
    }
}
