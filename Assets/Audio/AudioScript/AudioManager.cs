using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _MusicSource, _SFXSource;

    [SerializeField] private AudioClip[] source;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }


    public void StartMusic()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "MainMenu")
        {
            PlayMusic(source[0]);
        }
        if (sceneName == "GameScene")
        {
            PlayMusic(source[1]);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        _MusicSource.clip = clip;
        _MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        _SFXSource.PlayOneShot(clip);
    }
}
