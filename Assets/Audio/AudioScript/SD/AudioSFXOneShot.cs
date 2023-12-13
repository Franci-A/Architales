using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSFXOneShot : AudioScript
{

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

        AudioManager.Instance.PlaySFXWValues(audioStruct);

        Destroy(gameObject);

    }

    public void PlayWind()
    {
        m_selectedClip = GetClip(false);

        AudioManager.AudioStruct audioStruct = new AudioManager.AudioStruct();
        audioStruct.clip = m_selectedClip;
        audioStruct.volume = rndVol();
        audioStruct.pitch = rndPitch();
        audioStruct.timeSamples = 0;
        audioStruct.is3D = is3D;

        AudioManager.Instance.PlayWindWValues(audioStruct);

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
}
