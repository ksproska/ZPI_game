using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsContainer : MonoBehaviour
{
    public Bounds bounds;
    public BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        bounds = boxCollider.bounds;
    }
}
