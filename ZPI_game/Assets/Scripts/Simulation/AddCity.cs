using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCity : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private PointsContainer pointsContainer;

    public void Add()
    {
        var city = Instantiate(pointPrefab, map.transform.position, Quaternion.identity, FindObjectOfType<Canvas>().transform);
        city.transform.parent = pointsContainer.gameObject.transform;
        city.GetComponent<City>().container = pointsContainer;
    }
}
