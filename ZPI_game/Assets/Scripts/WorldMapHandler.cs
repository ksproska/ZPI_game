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
    private float prevTime;
    private float glitchRandom;
    public Image map;
    public Sprite glithched;
    public int glitchBeginningLevel;
    public GameObject wires;
    [SerializeField] private AudioSource source;
    private int glitchLevel;
    private float glithprobability;
    // Start is called before the first frame update
    void Start()
    {
        glitchLevel = Math.Max(0,(LevelMap.GetListOfLevels(CurrentGameState.CurrentSlot).Max(level => level.LevelNumber)) - glitchBeginningLevel);
        prevTime = Time.time;
        source.volume = CurrentGameState.EffectsVolume;
    }

    // Update is called once per frame
    void Update()
    {
        glitchRandom = UnityEngine.Random.Range(0.0f, 1.0f);

        if (Time.time - prevTime > 1.0f )
        {
            if(glitchRandom < glitchLevel * 0.01f)
            {
                StartCoroutine(glitch());
            }
            prevTime = Time.time;
        }
    }

    IEnumerator glitch()
    {
        source.Play();
        Sprite notGlithched = map.sprite;

        map.sprite = glithched;
        wires.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        map.sprite = notGlithched;
        wires.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        map.sprite = glithched;
        wires.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        map.sprite = notGlithched;
        wires.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        map.sprite = glithched;
        wires.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        map.sprite = notGlithched;
        wires.SetActive(true);
    }
}
