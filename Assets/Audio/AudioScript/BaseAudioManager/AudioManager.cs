using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    private static AudioManager instance;
    public static AudioManager Instance { get => instance; }

    [SerializeField] private AudioSource _MusicSource, _SFXSource, _WindSource;

    private float saveSoundVolume;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        saveSoundVolume = _MusicSource.volume;
    }

    public void StopMusic()
    {
        saveSoundVolume = _MusicSource.volume;
        _MusicSource.volume = 0;
    }

    public void PlayMusic(AudioClip clip)
    {
        _MusicSource.clip = clip;
        _MusicSource.UnPause();
        _MusicSource.Play();
        _MusicSource.loop = true;
    }

    public void PlayMusicSetSound(AudioClip clip)
    {
        _MusicSource.volume = saveSoundVolume;
        _MusicSource.clip = clip;
        _MusicSource.UnPause();
        _MusicSource.Play();
        _MusicSource.loop = false;
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
    public void PlaySFXWValues(AudioStruct audio, bool usePlay)
    { 
        if(audio.clip == null) return;

        _SFXSource.pitch = audio.pitch;
        _SFXSource.volume = audio.volume;
        _SFXSource.timeSamples = audio.timeSamples;

        if (!usePlay) _SFXSource.PlayOneShot(audio.clip);
        else
        {
            _SFXSource.clip = audio.clip;
            _SFXSource.Play();
        }

        if (audio.is3D) _SFXSource.spatialBlend = 1;
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
