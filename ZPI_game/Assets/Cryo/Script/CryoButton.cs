using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CryoButton : MonoBehaviour
{
    [SerializeField] private float blinkProbability = 0.1f;
    System.Random rnd = new();
    private float lastUpdate = 0;
}
