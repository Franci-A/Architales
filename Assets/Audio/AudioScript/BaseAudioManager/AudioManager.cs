using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _MusicSource, _SFXSource;

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
    public void PlaySFXRandom(AudioStruct audio)
    {
        _SFXSource.pitch = Random.Range(audio.randomPitch.x, audio.randomPitch.y);
        _SFXSource.volume = Random.Range(audio.randomVolume.x, audio.randomVolume.y);
        _SFXSource.PlayOneShot(audio.clip);
    }

    public void PauseMusic()
    {
        _MusicSource.Pause();
    }


    public struct AudioStruct
    {
        public AudioClip clip;
        public Vector2 randomVolume;
        public Vector2 randomPitch;
    }
}
