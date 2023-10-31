using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private Vector3 cameraRotation;
    private float currentRotation;
    [SerializeField] private float speed;
    private float previsousPosition;

    bool updateRotation = false;

    private void Start()
    {
        cameraRotation = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            updateRotation = true;
        previsousPosition = Input.mousePosition.x;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            updateRotation = false;
        }

        if (updateRotation)
        {
            float direction = Input.mousePosition.x - previsousPosition;
            previsousPosition = Input.mousePosition.x;
            currentRotation += speed * Time.deltaTime * direction;
            cameraTransform.rotation = quaternion.Euler(cameraRotation.x, cameraRotation.y + currentRotation, cameraRotation.z);
        }
    }
}
