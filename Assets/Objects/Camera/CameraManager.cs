using HelperScripts.EventSystem;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform targetZoom;


    //Rotation
    private Vector3 cameraRotation;
    private float currentRotationX, currentRotationY;
    bool updateRotation = false;
    private bool smoothBracking = false;
    private bool cameraInvertion = false;


    [Header("Position / Speed")]
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float rotationMinClamp;
    [SerializeField] private float rotationMaxClamp;

    [Header("Elevator")]
    [SerializeField] private float elevatorSpeed;
    [SerializeField] private float elevatorMouseSpeed;
    [SerializeField] private float elevatorMinClamp;
    [SerializeField] private int elevatorMaxClampOffset = 3;


    private float mousePositionX, mousePositionY;

    private float previsousPositionX, previsousPositionY;
    private float directionx, directiony;
    private bool rightClickPushed; // check if rightclick is pushed
    private bool rightClickOnce; // to start rotate (but once)

    private float previsousPositionYVertical;
    private float directionyVertical;
    private bool leftClickPushed; // check if rightclick is pushed
    private bool leftClickOnce; // to start rotate (but once)
    bool updatePosition = false;
    int higherBlock = 1;
    [SerializeField] private EventScriptable onPiecePlaced;
    [SerializeField] private BoolVariable isPlayerActive;


    [Header("Zoom")]
    [SerializeField] private float zoomMinClamp;
    [SerializeField] private float zoomMaxClamp;
    [SerializeField, Range (0, 1f)] private float zoomSpeed;
    private bool zoomActive = false;
    private Vector3 velocity;
    private float targetT;

    //Vertical Input
    private float verticalInput;

    [Header("Mouse Check")]
    private float maxDistance = 15;
    private LayerMask cubeLayer;

    [Header("AFKCamera")]
    [SerializeField] private GameObject ui;
    [SerializeField, Range(0, 1f)] private float distanceAFKZoom;
    [SerializeField] private float horizontalAFKSpeed;
    [SerializeField] private float speedAFKZoom;
    [SerializeField] private float afkTimer;
    private float timer;



    private void Start()
    {
        cameraRotation = transform.rotation.eulerAngles;
        maxDistance = Grid3DManager.Instance.MaxDistance;
        cubeLayer = Grid3DManager.Instance.CubeLayer;

        onPiecePlaced.AddListener(UpdateHigherBlock);
    }

    private void OnDestroy()
    {
        onPiecePlaced.RemoveListener(UpdateHigherBlock);
    }

    void Update()
    {
        if (!isPlayerActive) return;

        CheckRotation();

        VerticalMovement();

        if (zoomActive)
            ZoomUpdate();

        timer += Time.deltaTime;
        if (timer >= afkTimer)
            AFKCamera();
        else if (ui.GetComponent<CanvasGroup>().alpha < 1f)
            ui.GetComponent<CanvasGroup>().alpha += 0.05f;
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

            resetTimer();

            #region CAMERA CLAMP
            if (cameraRotation.x + currentRotationX > rotationMaxClamp)
            {
                if (!cameraInvertion)
                {
                    if (directionx < 0)
                    {
                        currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                        cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                    }
                    else if (directionx > 0)
                        cameraTransform.rotation = quaternion.Euler(rotationMaxClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
                else
                {
                    if (directionx > 0)
                    {
                        currentRotationX -= verticalSpeed * Time.deltaTime * directionx;
                        cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                    }
                    else if (directionx < 0)
                        cameraTransform.rotation = quaternion.Euler(rotationMaxClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
            }
            else if (cameraRotation.x + currentRotationX < rotationMinClamp)
            {
                if (!cameraInvertion)
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
                    if (directionx < 0)
                    {
                        currentRotationX -= verticalSpeed * Time.deltaTime * directionx;
                        cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                    }
                    else if (directionx > 0)
                        cameraTransform.rotation = quaternion.Euler(rotationMinClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
                
            }
            else 
            {
                if (!cameraInvertion)
                    currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                else
                    currentRotationX -= verticalSpeed * Time.deltaTime * directionx;

                cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
            }
            #endregion
        }


        #region SMOOTH SLOW
        if (smoothBracking)
        {

            directionx -= directionx * 0.05f;
            directiony -= directiony * 0.05f;

            if (directionx <= 0.01f && directionx >= -0.01f || directiony <= 0.01f && directiony >= -0.01f)
                smoothBracking = false;

            currentRotationY += horizontalSpeed * Time.deltaTime * directiony;


            if (cameraRotation.x + currentRotationX > rotationMaxClamp)
            {
                if (!cameraInvertion)
                {
                    if (directionx < 0)
                    {
                        currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                        cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                    }
                    else if (directionx > 0)
                        cameraTransform.rotation = quaternion.Euler(rotationMaxClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
                else
                {
                    if (directionx > 0)
                    {
                        currentRotationX -= verticalSpeed * Time.deltaTime * directionx;
                        cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                    }
                    else if (directionx < 0)
                        cameraTransform.rotation = quaternion.Euler(rotationMaxClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
            }
            else if (cameraRotation.x + currentRotationX < rotationMinClamp)
            {
                if (!cameraInvertion)
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
                    if (directionx < 0)
                    {
                        currentRotationX -= verticalSpeed * Time.deltaTime * directionx;
                        cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
                    }
                    else if (directionx > 0)
                        cameraTransform.rotation = quaternion.Euler(rotationMinClamp, cameraRotation.y + currentRotationY, cameraRotation.z);
                }
            }
            else
            {
                if (!cameraInvertion)
                    currentRotationX += verticalSpeed * Time.deltaTime * directionx;
                else
                    currentRotationX -= verticalSpeed * Time.deltaTime * directionx;

                cameraTransform.rotation = quaternion.Euler(cameraRotation.x + currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);
            }
        }
        #endregion
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

        if (leftClickPushed && leftClickOnce)
        {
            leftClickOnce = false;

            updatePosition = true;
            previsousPositionYVertical = mousePositionY;
        }
        else if (!leftClickPushed) updatePosition = false;

        if (updatePosition)
        {
            directionyVertical = mousePositionY - previsousPositionYVertical;
            previsousPositionYVertical = mousePositionY;
            cameraTransform.position = new Vector3(0, cameraTransform.position.y + Time.deltaTime * (directionyVertical * elevatorMouseSpeed), 0);
            timer = 0;
        }

        var yPositionClamped = Mathf.Clamp(cameraTransform.position.y, elevatorMinClamp, higherBlock + elevatorMaxClampOffset);
        cameraTransform.position = new Vector3(0, yPositionClamped, 0);
    }

    private void Zoom(float value)
    {
        //ZOOM CONDITIONS
        targetT -= zoomSpeed * value;
        targetT = Mathf.Clamp(targetT, 0, 1);
        zoomActive = true;
    }

    private void ZoomUpdate()
    {
        //ZOOM UPDATE
        var minPos = targetZoom.position - mainCamera.transform.forward * zoomMinClamp;
        var maxPos = targetZoom.position - mainCamera.transform.forward * zoomMaxClamp;
        var targetPosition = Vector3.Lerp(minPos, maxPos, targetT);

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, 0.25f);

        if (velocity.magnitude <= 0.01f)
            zoomActive = false;

        resetTimer();
    }

    public void ZoomSetClamp(float upMin, float upMax)
    {
        zoomMinClamp = upMin;
        zoomMaxClamp = upMax;
    }

    void UpdateHigherBlock()
    {
        higherBlock = Grid3DManager.Instance.GetHigherBlock;
    }

    public void CameraInvertion()
    {
        if (!cameraInvertion)
            cameraInvertion = true;
        else
            cameraInvertion = false;
    }

    public void resetTimer()
    {
        timer = 0;
    }

    private void AFKCamera()
    {
        if (ui.GetComponent<CanvasGroup>().alpha > 0f)
            ui.GetComponent<CanvasGroup>().alpha -= 0.01f;
        currentRotationY -= horizontalAFKSpeed * Time.deltaTime;

        cameraTransform.rotation = quaternion.Euler(currentRotationX, cameraRotation.y + currentRotationY, cameraRotation.z);

        var minPos = targetZoom.position - mainCamera.transform.forward * zoomMinClamp;
        var maxPos = targetZoom.position - mainCamera.transform.forward * zoomMaxClamp;
        var targetPosition = Vector3.Lerp(minPos, maxPos, distanceAFKZoom);
        targetT = distanceAFKZoom;

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, speedAFKZoom);
    }



    //INPUTS
    public void LeftClickInput(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
        {
            if (context.performed)
            {
                leftClickPushed = true;
                leftClickOnce = true;
            }
            else if (context.canceled)
            {
                leftClickPushed = false;
            }
        }
    }
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
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, maxDistance, cubeLayer))
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
