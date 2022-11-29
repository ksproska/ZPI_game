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
        var parentRectTransform = pointsContainer.gameObject.GetComponent<RectTransform>();
        var city = Instantiate(pointPrefab, pointsContainer.transform);
        var pointTransform = city.GetComponent<RectTransform>();
        pointTransform.SetParent(parentRectTransform);
        pointTransform.anchorMax = new(0.5f, 0.5f);
        pointTransform.anchorMin = new(0.5f, 0.5f);
        pointTransform.pivot = new Vector2(0.5f, 0.5f);
        pointTransform.anchoredPosition = new Vector3(250, 230, 0);
        pointTransform.anchorMax = new(0, 0);
        pointTransform.anchorMin = new(0, 0);
        
        // var city = Instantiate(pointPrefab, map.transform.position, Quaternion.identity, FindObjectOfType<Canvas>().transform);
        // city.transform.SetParent(pointsContainer.gameObject.transform);
        // city.GetComponent<City>().container = pointsContainer;
    }
}
