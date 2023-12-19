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

    [SerializeField] private AudioSource _MusicSource, _SFXSource, _WindSource, _BirdSource, _GrassSource, _EagleSource, _DragonSource;

    private float saveSoundVolume;

    public enum AmbianceType
    {
        Wind, 
        Bird,
        Grass,
        Eagle,
        Dragon
    }

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

    public void PlayAmbiance(AudioStruct audio, AmbianceType ambianceType)
    {
        if (audio.clip == null) return;

        AudioSource audioSource = null;

        switch (ambianceType)
        {
            case AmbianceType.Wind:
                audioSource = _WindSource;
                break;
            case AmbianceType.Bird:
                audioSource = _BirdSource;
                break;
            case AmbianceType.Grass:
                audioSource = _GrassSource;
                break;
            case AmbianceType.Eagle:
                audioSource = _EagleSource;
                break;
            case AmbianceType.Dragon:
                audioSource = _DragonSource;
                break;
            default:
                break;
        }

        audioSource.pitch = audio.pitch;
        audioSource.volume = audio.volume;
        audioSource.timeSamples = audio.timeSamples;
        audioSource.PlayOneShot(audio.clip);

        if (audio.is3D) audioSource.spatialBlend = 1;
        else audioSource.spatialBlend = 0;
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
