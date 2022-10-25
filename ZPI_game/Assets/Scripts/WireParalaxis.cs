using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireParalaxis : MonoBehaviour
{
    private float cameraStartPosition;
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        cameraStartPosition = camera.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaPosition = cameraStartPosition - camera.transform.position.y;
        cameraStartPosition = camera.transform.position.y;
        transform.position += new Vector3 (0, deltaPosition * 0.3f);
    }
}
