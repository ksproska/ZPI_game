using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingComponent : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SlideOut()
    {
        StartCoroutine(SlideLoop(-10, 2));
    }

    private IEnumerator SlideLoop(float ammount, float time)
    {
        float currentTime = 0;
        while(currentTime < time)
        {
            currentTime += Time.deltaTime;
            var position = rectTransform.position;
            rectTransform.position = new Vector3(position.x + ammount, position.y, position.z);
            print(rectTransform.position);
            yield return null;
        }
    }
}
