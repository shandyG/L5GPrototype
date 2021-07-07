using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private GameObject camera;

    void Start()
    {
        camera = GameObject.Find("Main Camera");
    }

    void Update()
    {
        transform.LookAt(camera.transform);
        transform.Rotate(new Vector3(0f, 180f, 0f));

    }
}
