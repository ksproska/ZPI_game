using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireParalaxis : MonoBehaviour
{
    private float cameraStartPosition;
    public GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        cameraStartPosition = mainCamera.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaPosition = cameraStartPosition - mainCamera.transform.position.y;
        cameraStartPosition = mainCamera.transform.position.y;
        transform.position += new Vector3 (0, deltaPosition * 0.3f);
    }
}
