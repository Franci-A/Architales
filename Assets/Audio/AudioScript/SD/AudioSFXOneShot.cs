using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSFXOneShot : MonoBehaviour
{
    //prrivate variables
    private float m_clipLength;
    private AudioClip m_previousClip;
    private AudioClip m_selectedClip;

    //public variables
    public List<AudioClip> _audioClipList;

    

    [Range(0, 1)]
    public float rndVolMin;
    [Range(0, 1)]
    public float rndVolMax;

    [Space]

    [Range(0, 2)]
    public float rndPitchMin;
    [Range(0, 2)]
    public float rndPitchMax;

    // Start is called before the first frame update
    void Start()
    {
        //Prevent script from continuing if there is no audioclips in the list
        if (_audioClipList.Count == 0)
            return;

        PlaySound();
    }


    IEnumerator WaitForClipLength()
    {
        yield return new WaitForSeconds(m_clipLength);
        Destroy(this.gameObject);
    }

    float rndPitch()
    {
        return Random.Range(rndPitchMin, rndPitchMax);
    }

    float rndVol()
    {
        return Random.Range(rndVolMin, rndVolMax);
    }

    public void PlaySound()
    {
        if(_audioClipList.Count == 0)
        {
            return;
        }
        else if (m_selectedClip == null)
            m_selectedClip = _audioClipList[Random.Range(0, _audioClipList.Count - 1)];

        AudioManager.AudioStruct audioStruct = new AudioManager.AudioStruct();
        audioStruct.clip = m_selectedClip;
        audioStruct.volume = rndVol();
        audioStruct.pitch = rndPitch();

        AudioManager.Instance.PlaySFXWValues(audioStruct);

        Destroy(gameObject);


    }
    public AudioClip GetClip()
    {
        // return no audioclip if list is empty
        if (_audioClipList.Count == 0)
            return null;
        //return the only one audioclip in the list
        else if(_audioClipList.Count == 1)
            return _audioClipList[0];

        //if there is no previous audioclip saved from the RFX Manager return a random clip
        if(m_previousClip == null)
        {
            m_selectedClip = _audioClipList[Random.Range(0, _audioClipList.Count - 1)];
            return m_selectedClip;
        }
        //remove all audioclips similar to the previous clip
        List<int> indexesToRemove = new List<int>();
        int i = 0;
        foreach(AudioClip clip in _audioClipList)
        {
            if (clip == m_previousClip)
                indexesToRemove.Add(i);
            i++;
        }
        foreach (int index in indexesToRemove)
            _audioClipList.RemoveAt(index);

        //pick a clip in the remaining clip list
        m_selectedClip = _audioClipList[Random.Range(0, _audioClipList.Count - 1)];
        return m_selectedClip;

    }
    public void SetPreviousClip(AudioClip clip)
    {
        m_previousClip = clip;
    }
}
