using CurrentState;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private GoToScene goToScene;
    [SerializeField] private AudioSource audiosource;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private float timeToWait = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        var volume = CurrentGameState.Instance.EffectsVolume * 0.5f;
        audiosource.volume = volume;
        if (CurrentGameState.Instance.AreEffectsOn)
        {
            audiosource.Play();
        }
        StartCoroutine(EndAfterSeconds(time));
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToWait -= Time.deltaTime;
        if (timeToWait<=0.0f)
        {
            title.alpha += 0.5f * Time.deltaTime;
        }
    }
    IEnumerator EndAfterSeconds(float secs)
    {
        yield return new WaitForSeconds(secs);
        goToScene.FadeOutScene();
    }
}
