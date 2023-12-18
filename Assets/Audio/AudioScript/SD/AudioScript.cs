using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] protected List<AudioClip> _audioClipList = new List<AudioClip>();


    [Header("Clip values")]
    [Range(0, 1), SerializeField] protected float rndVolMin;
    [Range(0, 1), SerializeField] protected float rndVolMax;

    [Space]

    [Range(0, 2), SerializeField] protected float rndPitchMin;
    [Range(0, 2), SerializeField] protected float rndPitchMax;

    [SerializeField] protected bool is3D;



    protected AudioClip m_selectedClip;
    protected AudioClip m_previousClip;

    protected virtual void Awake()
    {
        //Prevent script from continuing if there is no audioclips in the list
        if (_audioClipList.Count == 0)
            return;
    }

    protected float rndPitch()
    {
        return Random.Range(rndPitchMin, rndPitchMax);
    }

    protected float rndVol()
    {
        return Random.Range(rndVolMin, rndVolMax);
    }

    public virtual void PlaySound()
    {

    }

    public virtual AudioClip GetClip(bool canHaveSameSoundTwice)
    {
        if (_audioClipList.Count == 0) return null;

        AudioClip selectedClip = _audioClipList[Random.Range(0, _audioClipList.Count)];

        if (_audioClipList.Count == 1 || canHaveSameSoundTwice) return selectedClip;


        while (selectedClip == m_previousClip)
        {
            selectedClip = _audioClipList[Random.Range(0, _audioClipList.Count)];
        }

        m_previousClip = selectedClip;
        if (selectedClip == null)
        {
            return null;
        }

        return selectedClip;

    }
}
