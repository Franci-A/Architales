using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private Vector3 cameraRotation;
    private float currentRotationx, currentRotationy;
    [SerializeField] private float speed;
    private float previsousPositionx, previsousPositiony;

    private float lockRotationx;

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
            previsousPositiony = Input.mousePosition.x;
            previsousPositionx = Input.mousePosition.y;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            updateRotation = false;
        }

        if (updateRotation)
        {
            float directionx = Input.mousePosition.y - previsousPositionx;
            previsousPositionx = Input.mousePosition.y;
            currentRotationx += speed * Time.deltaTime * directionx;

            float directiony = Input.mousePosition.x - previsousPositiony;
            previsousPositiony = Input.mousePosition.x;
            currentRotationy += speed * Time.deltaTime * directiony;


            if (cameraRotation.x + currentRotationx > 0.65)
            {
                lockRotationx = 0.65f;
                cameraTransform.rotation = quaternion.Euler(lockRotationx, cameraRotation.y + currentRotationy, cameraRotation.z);
            } 
            else if (cameraRotation.x + currentRotationx < -0.8)
            {
                lockRotationx = -0.8f;
                cameraTransform.rotation = quaternion.Euler(lockRotationx, cameraRotation.y + currentRotationy, cameraRotation.z);
            } 
            else
                cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationx, cameraRotation.y + currentRotationy, cameraRotation.z);

        }
    }
}
