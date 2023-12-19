using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSFXOneShot : AudioScript
{
    [SerializeField] private bool usePlay;

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
        audioStruct.timeSamples = 0;
        audioStruct.is3D = is3D;

        AudioManager.Instance.PlaySFXWValues(audioStruct, usePlay);

        Destroy(gameObject);

    }

    public void PlayAmbiance(AudioManager.AmbianceType type)
    {
        m_selectedClip = GetClip(false);

        AudioManager.AudioStruct audioStruct = new AudioManager.AudioStruct();
        audioStruct.clip = m_selectedClip;
        audioStruct.volume = rndVol();
        audioStruct.pitch = rndPitch();
        audioStruct.timeSamples = 0;
        audioStruct.is3D = is3D;

        AudioManager.Instance.PlayAmbiance(audioStruct, type);

        Destroy(gameObject);

    }

    public void SetPreviousClip(AudioClip clip)
    {
        m_previousClip = clip;
    }

    public void AddClip(AudioClip clip)
    {
        _audioClipList.Add(clip);
    }

/*
    public void ChangeClipAndSpeed(AudioClip clip, float speed)
    {
        m_selectedClip = clip;
        rndPitchMax = speed;
        rndPitchMax = speed;
    }*/
}
