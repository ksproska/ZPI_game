using System;
using System.Collections.Generic;
using System.Linq;
using CurrentState;
using DeveloperUtils;
using Maps;
using UnityEngine;

namespace Challenges
{
    public class StaticCitiesContainer: MonoBehaviour
    {
        [SerializeField] private StaticCity mock;

        private void Start()
        {
            // var points = LocalMaps.Instance.GetMapById(CurrentGameState.Instance.CurrentMapId).Points;
            var points = LocalMaps.Instance.GetMapById(0).Points;
            var parentRectTransform = GetComponent<RectTransform>();
            foreach (var (x, y) in points)
            {
                var staticCity = Instantiate(mock, transform);
                var pointTransform = staticCity.GetComponent<RectTransform>();
                pointTransform.SetParent(parentRectTransform);
                pointTransform.anchoredPosition = new Vector3(x, y, 0);
                pointTransform.pivot = new Vector2(0.5f, 0.5f);
            }
        }
    }
}