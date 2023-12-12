using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXRandomContainer : AudioScript
{
    [Header("Start Position")]
    [Range(0, 1), SerializeField] private float rndStartMin;
    [Range(0, 1), SerializeField] private float rndStartMax;

    protected override void Awake()
    {
        base.Awake();
        PlaySound();
    }

    public override void PlaySound()
    {
        m_selectedClip = GetClip(false);

        AudioManager.AudioStruct audioStruct = new AudioManager.AudioStruct();
        audioStruct.clip = m_selectedClip;
        audioStruct.volume = rndVol();
        audioStruct.pitch = rndPitch();
        audioStruct.timeSamples = (m_selectedClip.samples - 1) * Random.Range((int)rndStartMin, (int)rndStartMax);
        audioStruct.is3D = is3D;

        AudioManager.Instance.PlaySFXWValues(audioStruct);
    }    
}
