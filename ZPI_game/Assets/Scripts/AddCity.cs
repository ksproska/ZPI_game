using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCity : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject pointPrefab;

    public void Add()
    {
        Instantiate(pointPrefab, map.transform.position, Quaternion.identity, FindObjectOfType<Canvas>().transform);
    }
}
