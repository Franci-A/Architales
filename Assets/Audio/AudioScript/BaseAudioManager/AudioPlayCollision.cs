using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class AudioPlayCollision : AudioScript
{

    [Layer, SerializeField] private string layer;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(layer))
        {
            PlaySound();
        }
    }


    public override void PlaySound()
    {
        m_selectedClip = GetClip(false);

        AudioManager.AudioStruct audioStruct = new AudioManager.AudioStruct();
        audioStruct.clip = m_selectedClip;
        audioStruct.volume = rndVol();
        audioStruct.pitch = rndPitch();
        audioStruct.timeSamples = 0;
        audioStruct.is3D = is3D;

        AudioManager.Instance.PlaySFXWValues(audioStruct);

    }
}
