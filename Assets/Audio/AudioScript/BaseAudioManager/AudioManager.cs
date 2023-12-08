using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    private static AudioManager instance;
    public static AudioManager Instance { get => instance; }

    [SerializeField] private AudioSource _MusicSource, _SFXSource;

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
    }

    public void PlaySFX(AudioClip clip)
    {
        _SFXSource.PlayOneShot(clip);
    }

    //public void PlaySFXRandom(AudioClip clip, Vector2 randomVolume, Vector2 randomPitch)
    public void PlaySFXWValues(AudioStruct audio)
    {
        _SFXSource.pitch = audio.pitch;
        _SFXSource.volume = audio.volume;
        _SFXSource.PlayOneShot(audio.clip);
    }

    public void PauseMusic()
    {
        _MusicSource.Pause();
    }


    public struct AudioStruct
    {
        public AudioClip clip;
        public float volume;
        public float pitch;
    }
}
