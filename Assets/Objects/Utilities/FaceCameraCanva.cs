using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraCanva : MonoBehaviour
{

    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(transform.position - cam.transform.position);

    }
}
