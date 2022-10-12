using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxMenu : MonoBehaviour
{
    Vector2 position;
    Vector2 startPosition;

    [SerializeField] int moveModifier;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float posX = Mathf.Lerp(transform.position.x, startPosition.x + position.x * moveModifier, 2f * Time.deltaTime);
        float posY = Mathf.Lerp(transform.position.y, startPosition.y + position.y * moveModifier, 2f * Time.deltaTime);
        transform.position = new Vector3(posX, posY, 0f);
    }
}
