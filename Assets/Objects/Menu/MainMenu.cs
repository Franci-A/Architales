using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI resText;
    [SerializeField] List<Vector2> resolutionList = new List<Vector2>(); // x = width, y = height
    private int resId;
    private bool boolFullScreen;


    private void Start()
    {
        LoadSliderValue();
        GetScreenValue();
        AudioManager.instance.StartMusic();
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
        UnityEngine.Application.Quit();
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

    void GetScreenValue()
    {
        if (!PlayerPrefs.HasKey("FullScreen")) PlayerPrefs.SetInt("FullScreen", 1);
        boolFullScreen = PlayerPrefs.GetInt("FullScreen") > 0 ? true : false;
        fullscreenToggle.isOn = boolFullScreen;

        if (!PlayerPrefs.HasKey("ResolutionId")) PlayerPrefs.SetInt("ResolutionId", 0);
        resId = PlayerPrefs.GetInt("ResolutionId");
        resText.text = $"{resolutionList[resId].x} x {resolutionList[resId].y}";

        ApplyGraphics();
    }

    public void ApplyGraphics()
    {
        PlayerPrefs.SetInt("FullScreen", boolFullScreen ? 1 : 0);
        PlayerPrefs.SetInt("ResolutionId", resId);

        UnityEngine.Screen.SetResolution((int)resolutionList[resId].x, (int)resolutionList[resId].y, fullscreenToggle.isOn);
    }

    public void SetFullscreen(bool _bool)
    {
        boolFullScreen = _bool;
    }

    public void ReduceRes()
    {
        resId--;
        if(resId < 0) resId =  0;

        resText.text = $"{resolutionList[resId].x} x {resolutionList[resId].y}";
    }

    public void IncreaseRes()
    {
        resId++;
        if (resId >= resolutionList.Count) resId = resolutionList.Count - 1;

        resText.text = $"{resolutionList[resId].x} x {resolutionList[resId].y}";
    }




}
