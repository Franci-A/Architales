using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private GameObject sfxOver;
    [SerializeField] private GameObject sfxClick;
    [SerializeField] private GameObject sfxClickPlay;

    public void LoadSliderValue()
    {
        if (!PlayerPrefs.HasKey("MasterVolume")) PlayerPrefs.SetFloat("MasterVolume", 1f);
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        SetMasterVolume(masterVolumeSlider.value);

        if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume(musicVolumeSlider.value);

        if (!PlayerPrefs.HasKey("SFXVolume")) PlayerPrefs.SetFloat("SFXVolume", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSFXVolume(sfxVolumeSlider.value);

    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
        //audioMixer.SetFloat("Master", Mathf.Lerp(-80 , 0,Mathf.Log(value +1)));
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        Debug.Log(value);
        audioMixer.SetFloat("Music", Mathf.Log10(value) * 20);
        //audioMixer.SetFloat("Music", Mathf.Lerp(-80, 0, Mathf.Log(value + 1)));
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        //audioMixer.SetFloat("SFX", Mathf.Lerp(-80, 0, Mathf.Log(value + 1)));
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void OnOverButton()
    {
        Instantiate(sfxOver);
    }

    public void OnClickButton()
    {
        Instantiate(sfxClick);
    }

    public void OnClickPlayButton()
    {
        Instantiate(sfxClickPlay);
    }
}
