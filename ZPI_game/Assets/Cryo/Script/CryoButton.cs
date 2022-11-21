using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CryoButton : MonoBehaviour
{
    [SerializeField] private CryoUI cryo;
    [SerializeField] private float secondsTillBlink;
    [SerializeField] private CryoExplanation explanationFrame;

    System.Random rnd = new();
    private float lastUpdate = 0;

    private void Start()
    {
        cryo.SetBothEyesTypes(Assets.Cryo.Script.EyeType.EyeBig);
    }

    private void FixedUpdate()
    {
        lastUpdate += Time.deltaTime;
        if(lastUpdate > secondsTillBlink)
        {
            secondsTillBlink = (float)(rnd.NextDouble() * 10 + 5);
            lastUpdate = 0;
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        cryo.SetBothEyesTypes(Assets.Cryo.Script.EyeType.Closed);
        yield return new WaitForSeconds(0.5f);
        cryo.SetBothEyesTypes(Assets.Cryo.Script.EyeType.EyeBig);
    }

    public void ToggleExplanationFrame()
    {
        gameObject.SetActive(false);
        explanationFrame.SetFrameActive(true);
        //if (explanationFrame.gameObject.activeSelf)
        //{
        //    explanationFrame.gameObject.SetActive(false);
        //    if(stopper != null)
        //    {
        //        stopper.gameObject.SetActive(false);
        //    }
        //}
        //else
        //{
        //    explanationFrame.gameObject.SetActive(true);
        //    if (stopper != null)
        //    {
        //        stopper.gameObject.SetActive(true);
        //    }
        //}
    }
}
