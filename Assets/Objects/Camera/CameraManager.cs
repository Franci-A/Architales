using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

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
    private bool smoothBracking = false;


    [Header("Position / Speed")]
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    private float previsousPositionX, previsousPositionY;
    private float mousePositionX, mousePositionY;
    private float directionx, directiony;

    private bool rightClickPushed; // check if rightclick is pushed
    private bool rightClickOnce; // to start rotate (but once)


    [Header("Zoom")]
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    private float zoom, velocity, valueZoom, minValueZoom, maxValueZoom;
    private bool zoomActive = false;


    //Vertical Input
    private float verticalInput;

    private void Start()
    {
        cameraRotation = transform.rotation.eulerAngles;
        zoom = mainCamera.fieldOfView;
    }

    void Update()
    {
        CheckRotation();

        VerticalMovement();

        if (zoomActive)
            ZoomUpdate();
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
            directionx = mousePositionY - previsousPositionX;
            previsousPositionX = mousePositionY;



            directiony = mousePositionX - previsousPositionY;
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


        //SMOOTH SLOW
        if (smoothBracking)
        {

            directionx -= directionx * 0.05f;
            directiony -= directiony * 0.05f;

            if (directionx <= 0.01f && directionx >= -0.01f || directiony <= 0.01f && directiony >= -0.01f)
                smoothBracking = false;

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
        //ZOOM CONDITIONS
        valueZoom = value;
        minValueZoom = (mainCamera.fieldOfView - valueZoom * 4f) - 3f;
        maxValueZoom = (mainCamera.fieldOfView - valueZoom * 4f) + 3f;
        zoomActive = true;
    }

    private void ZoomUpdate()
    {
        //ZOOM UPDATE
        zoom -= valueZoom * 4f;
        if (minValueZoom >= minZoom && maxValueZoom <= maxZoom)
        {
            zoom = Mathf.Clamp(zoom, minValueZoom, maxValueZoom);
            mainCamera.fieldOfView = Mathf.SmoothDamp(mainCamera.fieldOfView, zoom, ref velocity, 0.25f);
        }
        else
            zoomActive = false;

        if (zoom <= 0)
            zoomActive = false;
    }


    //INPUTS
    public void RightClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rightClickPushed = true;
            rightClickOnce = true;
            smoothBracking = false;
        }
        else if (context.canceled)
        {
            rightClickPushed = false;
            smoothBracking = true;
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
