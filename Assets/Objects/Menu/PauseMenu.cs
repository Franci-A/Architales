using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Scenes")]
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject option;
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private string gameSceneName;
    [SerializeField] private BoolVariable isPlayerActive;

    [Header("Audio")]
    [SerializeField] private UnityEvent playMusic;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Screen")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TextMeshProUGUI resText;
    [SerializeField] Resolution[] resolutionList; // x = width, y = height
    private int resId;
    private bool boolFullScreen;
    private CameraManager cameraManager;


    private void Start()
    {
        resolutionList = UnityEngine.Screen.resolutions;
        LoadSliderValue();
        GetScreenValue();
        isPlayerActive.SetValue(false);
    }

    #region Main
    public void StartGame()
    {
        isPlayerActive.SetValue(true);
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
        audioMixer.SetFloat("Master", Mathf.Lerp(-80, 0, Mathf.Log(value + 1)));
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("Music", Mathf.Lerp(-80, 0, Mathf.Log(value + 1)));
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX", Mathf.Lerp(-80, 0, Mathf.Log(value + 1)));
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    #endregion

    void GetScreenValue()
    {
        if (!PlayerPrefs.HasKey("FullScreen")) PlayerPrefs.SetInt("FullScreen", 1);
        boolFullScreen = PlayerPrefs.GetInt("FullScreen") > 0 ? true : false;
        fullscreenToggle.isOn = boolFullScreen;

        if (!PlayerPrefs.HasKey("ResolutionId")) PlayerPrefs.SetInt("ResolutionId", resolutionList.Length - 1);
        resId = PlayerPrefs.GetInt("ResolutionId");
        resText.text = $"{resolutionList[resId].width} x {resolutionList[resId].height}";

        ApplyGraphics();
    }

    public void ApplyGraphics()
    {
        PlayerPrefs.SetInt("FullScreen", boolFullScreen ? 1 : 0);
        PlayerPrefs.SetInt("ResolutionId", resId);

        UnityEngine.Screen.SetResolution((int)resolutionList[resId].width, (int)resolutionList[resId].height, fullscreenToggle.isOn);
    }

    public void SetFullscreen(bool _bool)
    {
        boolFullScreen = _bool;
    }

    public void SetInversion()
    {
        cameraManager = Camera.main.GetComponentInParent<CameraManager>();
        cameraManager.CameraInvertion();
    }

    public void ReduceRes()
    {
        resId--;
        if (resId < 0) resId = 0;

        resText.text = $"{resolutionList[resId].width} x {resolutionList[resId].height}";
    }

    public void IncreaseRes()
    {
        resId++;
        if (resId >= resolutionList.Length) resId = resolutionList.Length - 1;

        resText.text = $"{resolutionList[resId].width} x {resolutionList[resId].height}";
    }

    public void Resume()
    {
        //Time.timeScale = 1.0f;
        gameManager.ResumeGame();
    }

    public void Menu()
    {
        //Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
