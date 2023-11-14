using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    private bool smoothBracking = false;
    private float rangeMultiplier = 20;


    [Header("Position / Speed")]
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float rotationMinClamp;
    [SerializeField] private float rotationMaxClamp;

    [Header("Elevator")]
    [SerializeField] private float elevatorSpeed;
    [SerializeField] private float elevatorMinClamp;

    private float previsousPositionX, previsousPositionY;
    private float mousePositionX, mousePositionY;
    private float directionx, directiony;

    private bool rightClickPushed; // check if rightclick is pushed
    private bool rightClickOnce; // to start rotate (but once)


    [Header("Zoom")]
    [SerializeField] private float zoomMinClamp;
    [SerializeField] private float zoomMaxClamp;
    [SerializeField] private float zoomSpeed;
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


            /*//CAMERA SLOW
            if (cameraRotation.x + currentRotationX > rotationMaxClamp - 0.1f && cameraRotation.x + currentRotationX < rotationMaxClamp)
            {
                currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                Debug.Log(cameraRotation.x + currentRotationX);
                if (directionx < 0)
                    cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                else if (directionx > 0)
                    cameraTransform.rotation = quaternion.Euler((cameraRotation.x + currentRotationX) / Math.Abs((cameraRotation.x + currentRotationX) - rotationMaxClamp), cameraRotation.y + currentRotationY, cameraRotation.z);
            }
            else if (cameraRotation.x + currentRotationX < rotationMinClamp + 0.1f && cameraRotation.x + currentRotationX > rotationMinClamp)
            {
                currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                Debug.Log(cameraRotation.x + currentRotationX);
                if (directionx > 0)
                    cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                else if (directionx < 0)
                    cameraTransform.rotation = quaternion.Euler((cameraRotation.x + currentRotationX) / Math.Abs((cameraRotation.x + currentRotationX) - rotationMaxClamp), cameraRotation.y + currentRotationY, cameraRotation.z);
            }
            else*/




            //CAMERA CLAMP
            if (cameraRotation.x + currentRotationX > rotationMaxClamp)
            {
                if (directionx < 0)
                {
                    currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                    cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
                else if (directionx > 0)
                    cameraTransform.rotation = quaternion.Euler(rotationMaxClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
            }
            else if (cameraRotation.x + currentRotationX < rotationMinClamp)
            {
                if (directionx > 0)
                {
                    currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                    cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
                else if (directionx < 0)
                    cameraTransform.rotation = quaternion.Euler(rotationMinClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
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
                if (directionx < 0)
                {
                    currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                    cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
                else
                    cameraTransform.rotation = quaternion.Euler(rotationMaxClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
            }
            else if (cameraRotation.x + currentRotationX < -0.8)
            {
                if (directionx > 0)
                {
                    currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                    cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
                else
                    cameraTransform.rotation = quaternion.Euler(rotationMinClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
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
        if (verticalInput != 0)
        {
            if (verticalInput > 0 && cameraTransform.position.y <= elevatorMinClamp || verticalInput < 0 && cameraTransform.position.y >= elevatorMinClamp || verticalInput > 0) 
                cameraTransform.position = new Vector3(0, cameraTransform.position.y + Time.deltaTime * (Mathf.Sign(verticalInput) * elevatorSpeed), 0);
        }
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
        zoom -= valueZoom * zoomSpeed;
        if (minValueZoom >= zoomMinClamp && maxValueZoom <= zoomMaxClamp)
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
