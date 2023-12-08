using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class AudioPlayCollision : MonoBehaviour
{

    public AudioClip[] AudioClip;


    [Layer] public string layer;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(layer))
        {
            AudioManager.AudioStruct audioStruct;

            //AudioManager.Instance.PlaySFXWValues(audioStruct);
        }
    }
}
