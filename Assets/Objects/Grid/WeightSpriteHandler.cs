using HelperScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightSpriteHandler : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] FloatVariable onCameraMovedHeight;

    private void Update()
    {
        SetToCameraPosition();
    }

    private void SetToCameraPosition()
    {
        Vector3 position = transform.position;
        position.y = onCameraMovedHeight.value;
        transform.position = position;
    }
}
