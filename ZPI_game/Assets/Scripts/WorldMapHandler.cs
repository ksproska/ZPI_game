using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CurrentState;

public class WorldMapHandler : MonoBehaviour
{
    private float nextTime;
    private float modifier;
    public Image map;
    public Sprite glithched;
    public GameObject wires;
    [SerializeField] private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        modifier = Random.Range(5.0f, 10.0f);
        nextTime = Time.time + modifier;
        source.volume = CurrentGameState.EffectsVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTime)
        {

            modifier = Random.Range(5.0f, 100.0f);
            nextTime = Time.time + modifier;

            StartCoroutine(glitch());


        }
    }

    IEnumerator glitch()
    {
        source.Play();
        Sprite notGlithched = map.sprite;
        map.sprite = glithched;
        foreach(var wire in wires.GetComponentsInChildren<SpriteRenderer>())
        {
            wire.enabled = false;
        }
        yield return new WaitForSeconds(0.05f);
        map.sprite = notGlithched;
        foreach (var wire in wires.GetComponentsInChildren<SpriteRenderer>())
        {
            wire.enabled = true;
        }
        yield return new WaitForSeconds(0.1f);
        map.sprite = glithched;
        foreach (var wire in wires.GetComponentsInChildren<SpriteRenderer>())
        {
            wire.enabled = false;
        }
        yield return new WaitForSeconds(0.01f);
        map.sprite = notGlithched;
        foreach (var wire in wires.GetComponentsInChildren<SpriteRenderer>())
        {
            wire.enabled = true;
        }
        yield return new WaitForSeconds(0.1f);
        map.sprite = glithched;
        foreach (var wire in wires.GetComponentsInChildren<SpriteRenderer>())
        {
            wire.enabled = false;
        }
        yield return new WaitForSeconds(0.25f);
        map.sprite = notGlithched;
        foreach (var wire in wires.GetComponentsInChildren<SpriteRenderer>())
        {
            wire.enabled = true;
        }
    }
}
