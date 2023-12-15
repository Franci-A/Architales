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
            PlaySound();
        if (collision.gameObject.layer == LayerMask.NameToLayer(layer))
        {
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

        AudioManager.Instance.PlaySFXWValues(audioStruct, false);

    }

    public void SetData(AudioPlayCollision apc)
    {
        _audioClipList = apc._audioClipList;
        rndVolMin = apc.rndVolMin;
        rndVolMax = apc.rndVolMax;
        rndPitchMin = apc.rndPitchMin;
        rndPitchMax = apc.rndPitchMax;
        is3D = apc.is3D;
        layer = apc.layer;

    }
}
