using DeveloperUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTest : MonoBehaviour
{
    [SerializeField] City city1;
    [SerializeField] City city2;
    
    public void Show()
    {
        city1.Distance(city2).Debug();
    }


}
