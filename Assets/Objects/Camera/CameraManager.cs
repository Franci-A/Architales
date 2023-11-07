using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Camera mainCamera;


    //Rotation
    private Vector3 cameraRotation;
    private float currentRotationX, currentRotationY;
    bool updateRotation = false;
    private float lockRotationX;


    [Header("Position / Speed")]
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    private float previsousPositionX, previsousPositionY;
    private float mousePositionX, mousePositionY;

    private bool rightClickPushed; // check if rightclick is pushed
    private bool rightClickOnce; // to start rotate (but once)


    [Header("Zoom")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom, maxZoom;
    private bool limitZoom = false;


    //Vertical Input
    private float verticalInput;

    private void Start()
    {
        cameraRotation = transform.rotation.eulerAngles;
    }

    void Update()
    {
        CheckRotation();

        VerticalMovement();
    }


    private void CheckRotation()
    {
        if (rightClickPushed && rightClickOnce)
        {
            rightClickOnce = false;

            updateRotation = true;
            previsousPositionY = mousePositionX;
            previsousPositionX = mousePositionY;
        }
        else if (!rightClickPushed) updateRotation = false;

        if (updateRotation)
        {
            float directionx = mousePositionY - previsousPositionX;
            previsousPositionX = mousePositionY;
            


            float directiony = mousePositionX - previsousPositionY;
            previsousPositionY = mousePositionX;
            currentRotationY += horizontalSpeed * Time.deltaTime * directiony;


            if (cameraRotation.x + currentRotationX > 0.65)
            {
                lockRotationX = 0.6f;
                if (directionx < 0)
                {
                    currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                    cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
                else 
                    cameraTransform.rotation = quaternion.Euler(lockRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
            }
            else if (cameraRotation.x + currentRotationX < -0.8)
            {
                lockRotationX = -0.75f;
                if (directionx > 0)
                {
                    currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                    cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
                else 
                    cameraTransform.rotation = quaternion.Euler(lockRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
            }
            else 
            {
                currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
            }
        }
    }

    private void ResetCamera()
    {
        mainCamera.fieldOfView = 60;
        cameraTransform.rotation = quaternion.Euler(50, 0, 0);
        cameraTransform.position = new Vector3(0, 0, 0);
    }

    private void VerticalMovement()
    {
        if(verticalInput != 0) cameraTransform.position = new Vector3(0, cameraTransform.position.y + Time.deltaTime * (Mathf.Sign(verticalInput) * verticalSpeed), 0);
    }

    private void Zoom(float value)
    {
        //zoom
        if (!limitZoom)
            mainCamera.fieldOfView -= value * zoomSpeed;

        if (mainCamera.fieldOfView <= minZoom)
        {
            limitZoom = true;
            mainCamera.fieldOfView = minZoom + 0.01f;
        }
        else if (mainCamera.fieldOfView >= maxZoom)
        {
            limitZoom = true;
            mainCamera.fieldOfView = maxZoom - 0.01f;
        }
        else
            limitZoom = false;
    }




    //INPUTS
    public void RightClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rightClickPushed = true;
            rightClickOnce = true;
        }
        else if (context.canceled)
        {
            rightClickPushed = false;
        }
    }

    public void MousePositionInput(InputAction.CallbackContext context)
    {
        mousePositionX = context.ReadValue<Vector2>().x;
        mousePositionY = context.ReadValue<Vector2>().y;
    }

    public void ZoomInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Zoom(Mathf.Sign(context.ReadValue<float>()));
    }

    public void VerticalMovementInput(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled) verticalInput = context.ReadValue<float>();
    }

    public void ResetCameraInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        ResetCamera();
    }
}
