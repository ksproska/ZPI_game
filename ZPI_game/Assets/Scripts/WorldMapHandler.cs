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
    // Start is called before the first frame update
    void Start()
    {
        var donelevels = (LevelMap.GetListOfLevels(CurrentGameState.CurrentSlot).Where(level => level.IsFinished));
        var glitchLevel = Math.Max(0, donelevels.Max(level => level.LevelNumber) - glitchBeginningLevel);
        prevTime = Time.time;
        source.volume = CurrentGameState.EffectsVolume;
    }

    // Update is called once per frame
    void Update()
    {
        glitchRandom = UnityEngine.Random.Range(0.0f, 1.0f);

        if (Time.time - prevTime > 1.0f )
        {
            if(glitchRandom < glitchLevel * 0.05f)
            {
                StartCoroutine(glitch());
            }
            prevTime = Time.time;
        }
    }

    IEnumerator glitch()
    {
        int glithNumber = rnd.Next(0, 2);
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
