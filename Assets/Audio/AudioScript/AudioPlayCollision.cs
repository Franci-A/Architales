using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class AudioPlayCollision : MonoBehaviour
{
    [SerializeField] private UnityEvent playSound;

    [Layer] public string layer;

    private void OnCollisionEnter(Collision collision)
    {
        if (layer == "Cube")
        {
            playSound.Invoke();
        }
    }
}
