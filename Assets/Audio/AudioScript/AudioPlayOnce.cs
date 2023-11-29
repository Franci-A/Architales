using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayOnce : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    void Start()
    {
        AudioManager.instance.PlaySFX(_clip);
    }
}
