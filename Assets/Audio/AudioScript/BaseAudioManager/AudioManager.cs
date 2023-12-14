using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    private static AudioManager instance;
    public static AudioManager Instance { get => instance; }

    [SerializeField] private AudioSource _MusicSource, _SFXSource, _WindSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        _MusicSource.clip = clip;
        _MusicSource.UnPause();
        _MusicSource.Play();
        _MusicSource.loop = true;
    }

    public void PlayMusicOnce(AudioClip clip)
    {
        _MusicSource.loop = false;
        _MusicSource.clip = clip;
        _MusicSource.UnPause();
        _MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        _SFXSource.PlayOneShot(clip);
    }

    //public void PlaySFXRandom(AudioClip clip, Vector2 randomVolume, Vector2 randomPitch)
    public void PlaySFXWValues(AudioStruct audio)
    { 
        if(audio.clip == null) return;

        _SFXSource.pitch = audio.pitch;
        _SFXSource.volume = audio.volume;
        _SFXSource.timeSamples = audio.timeSamples;
        _SFXSource.PlayOneShot(audio.clip);

        if(audio.is3D) _SFXSource.spatialBlend = 1;
        else _SFXSource.spatialBlend = 0;
    }

    public void PlayWindWValues(AudioStruct audio)
    {
        if (audio.clip == null) return;

        _WindSource.pitch = audio.pitch;
        _WindSource.volume = audio.volume;
        _WindSource.timeSamples = audio.timeSamples;
        _WindSource.PlayOneShot(audio.clip);

        if (audio.is3D) _WindSource.spatialBlend = 1;
        else _WindSource.spatialBlend = 0;
    }

    public void PauseMusic()
    {
        _MusicSource.Pause();
    }

    [Serializable]
    public struct AudioStruct
    {
        public AudioClip clip;
        public float volume;
        public float pitch;
        public int timeSamples;
        public bool is3D;
    }
}
