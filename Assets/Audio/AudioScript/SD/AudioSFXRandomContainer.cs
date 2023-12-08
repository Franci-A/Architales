using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXRandomContainer : MonoBehaviour
{

    [Space]
    [Header("AudioSource & AudioClips")]
    [Space]

    public AudioSource AudioSource;
    //public AudioSource AS_RainSource;
    public AudioClip[] AudioClip;

    [Space]
    [Header ("Random Pitch & Volumes")]
    [Space]


    [Range(0, 2)]
    public float rndPitchMin = 1;
    [Range(0, 2)]
    public float rndPitchMax = 1;
    [Range(0, 2)]
    public float rndVolMin = 1;
    [Range(0, 2)]
    public float rndVolMax = 1;


    AudioClip previousClip;
    AudioClip clipPlay;

    // Start is called before the first frame update
    void Start()
    {
        clipPlay = GetClip(AudioClip);
        AudioSource.pitch = Random.Range(rndPitchMin, rndPitchMax);
        AudioSource.volume = Random.Range(rndVolMin, rndVolMax);
        int randomStartTime = Random.Range(0, clipPlay.samples - 1); //clip.samples is the lengh of the clip in samples
        AudioSource.timeSamples = randomStartTime;
        AudioSource.clip = clipPlay;
        Debug.Log(AudioSource.clip.name);
        if (clipPlay != null)
            AudioSource.Play();
    }
    AudioClip GetClip(AudioClip[] clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip = clipArray[Random.Range(0, clipArray.Length)];

        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip = clipArray[Random.Range(0, clipArray.Length)];

            attempts--;
        }

        previousClip = selectedClip;
        if (selectedClip == null)
        {
            return null;
        }

        return selectedClip;


    }
}
