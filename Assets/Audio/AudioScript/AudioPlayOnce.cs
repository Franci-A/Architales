using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioPlayOnce : MonoBehaviour
{
    [SerializeField] private UnityEvent playSound;

    void Start()
    {
        if (playSound != null)
            playSound.Invoke();
    }
}
