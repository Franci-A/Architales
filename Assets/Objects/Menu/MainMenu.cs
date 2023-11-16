using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MainMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject option;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Screen")]
    [SerializeField] private Toggle fullscreenToggle;
    private void Start()
    {
        LoadSliderValue();
        SetFSValue();
    }

    #region Main
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        main.SetActive(false);
        option.SetActive(true);
    }

    public void BackToMain()
    {
        main.SetActive(true);
        option.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion


    #region Audio
    public void LoadSliderValue()
    {
        if (!PlayerPrefs.HasKey("MasterVolume")) PlayerPrefs.SetFloat("MasterVolume", 0.7f);
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        SetMasterVolume(masterVolumeSlider.value);

        if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 0.7f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume(musicVolumeSlider.value);

        if (!PlayerPrefs.HasKey("SFXVolume")) PlayerPrefs.SetFloat("SFXVolume", 0.7f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSFXVolume(sfxVolumeSlider.value);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", -80 + value *100);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("Music", -80 + value * 100);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX", -80 + value * 100);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    #endregion

    public void SetFSValue()
    {
        if (!PlayerPrefs.HasKey("FullScreen")) PlayerPrefs.SetInt("FullScreen", 1);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("FullScreen") > 0 ? true : false;
        SetFullscreen(fullscreenToggle.isOn);
    }

    public void SetFullscreen(bool _bool)
    {
        Screen.fullScreen = _bool;
        PlayerPrefs.SetInt("FullScreen", _bool ? 1 : 0);
    }

}
