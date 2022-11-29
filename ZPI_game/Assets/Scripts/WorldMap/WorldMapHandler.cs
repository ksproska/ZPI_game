using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CurrentState;
using LevelUtils;
using System.Linq;
using System;

public class WorldMapHandler : MonoBehaviour
{

    System.Random rnd = new System.Random();
    private float prevTime;
    public Image map;
    public Sprite[] glithched;
    public AudioClip[] audioGlitches;
    public int glitchBeginningLevel;
    public GameObject wires;
    [SerializeField] private AudioSource source;
    private int glitchLevel;
    private int timeDelay;
    public GameObject mask;
    public GameObject content;

    [SerializeField] private GameObject challengeButton;
    [SerializeField] private GameObject lastLevel;
    // Start is called before the first frame update
    void Start()
    {
        var donelevels = LevelMap.Instance.GetListOfLevels(CurrentGameState.Instance.CurrentSlot).Where(level => level.IsFinished).ToList();
        var maxlvl = donelevels.Max(level => level.LevelNumber);
        if(donelevels.Count != 0)
        {
            glitchLevel = Math.Max(0, maxlvl - glitchBeginningLevel);
        }
        prevTime = Time.time;
        source.volume = CurrentGameState.Instance.EffectsVolume;
        timeDelay = rnd.Next(30, 60);

        if (LevelMap.Instance.IsLevelDone(lastLevel.name, CurrentGameState.Instance.CurrentSlot)){
            challengeButton.SetActive(true);
        }
        else
        {
            challengeButton.SetActive(false);
        }
        var maxpos = FindObjectsOfType<LvlButton>().Max(level => level.GetComponent<RectTransform>().anchoredPosition.y);
        var minpos = FindObjectsOfType<LvlButton>().Min(level => level.GetComponent<RectTransform>().anchoredPosition.y);
        var maxLevelPos = FindObjectsOfType<LvlButton>().Where(level => LevelUtils.LevelMap.Instance.IsLevelDone(level.name, CurrentGameState.Instance.CurrentSlot)).Max(level => level.GetComponent<RectTransform>().anchoredPosition.y);
        //var lastDoneLevel = GameObject.Find(donelevels.Where(level => level.LevelNumber == donelevels.Max(level => level.LevelNumber)).First().LevelName);
        var lastDoneLevel = FindObjectsOfType<LvlButton>().Where(level => Math.Abs(maxLevelPos - level.GetComponent<RectTransform>().anchoredPosition.y) < 0.01).First();
        mask.GetComponent<ScrollRect>().verticalNormalizedPosition = (lastDoneLevel.GetComponent<RectTransform>().anchoredPosition.y - minpos)/(maxpos - minpos);
        Debug.Log((lastDoneLevel.GetComponent<RectTransform>().anchoredPosition.y - minpos) / (maxpos - minpos));
    }

    // Update is called once per frame
    void Update()
    {

        if (glitchLevel > 0)
        {
            if(Time.time - prevTime > timeDelay - glitchLevel)
            {
                StartCoroutine(glitch());
                prevTime = Time.time;
                timeDelay = rnd.Next(30, 60);
            }

        }
        Debug.Log(mask.transform.position);
    }

    IEnumerator glitch()
    {
        int glithNumber = rnd.Next(0, audioGlitches.Count());
        source.clip = audioGlitches[glithNumber];
        source.Play();
        Sprite notGlithched = map.sprite;

        map.sprite = glithched[glithNumber];
        wires.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        map.sprite = notGlithched;
        wires.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        map.sprite = glithched[glithNumber];
        wires.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        map.sprite = notGlithched;
        wires.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        map.sprite = glithched[glithNumber];
        wires.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        map.sprite = notGlithched;
        wires.SetActive(true);
    }
}
