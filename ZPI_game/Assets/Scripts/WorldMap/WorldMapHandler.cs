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
    private float glitchRandom;
    public Image map;
    public Sprite[] glithched;
    public AudioClip[] audioGlitches;
    public int glitchBeginningLevel;
    public GameObject wires;
    [SerializeField] private AudioSource source;
    private int glitchLevel;
    private int timeDelay;

    [SerializeField] private GameObject chellengeButton;
    [SerializeField] private GameObject lastLevel;
    // Start is called before the first frame update
    void Start()
    {
        var donelevels = LevelMap.Instance.GetListOfLevels(CurrentGameState.Instance.CurrentSlot).Where(level => level.IsFinished).ToList();
        if(donelevels.Count != 0)
        {
            glitchLevel = Math.Max(0, donelevels.Max(level => level.LevelNumber) - glitchBeginningLevel);
        }
        prevTime = Time.time;
        source.volume = CurrentGameState.Instance.EffectsVolume;
        timeDelay = rnd.Next(10, 20);

        if (LevelMap.Instance.IsLevelDone(lastLevel.name, CurrentGameState.Instance.CurrentSlot)){
            chellengeButton.SetActive(true);
        }
        else
        {
            chellengeButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        glitchRandom = UnityEngine.Random.Range(0.0f, 1.0f);

        if (glitchLevel > 0)
        {
            if(Time.time - prevTime > timeDelay - glitchLevel)
            {
                StartCoroutine(glitch());
                prevTime = Time.time;
                timeDelay = rnd.Next(10, 20);
            }

        }
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