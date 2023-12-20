using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject option;
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private string gameSceneName;
    [SerializeField] private BoolVariable isPlayerActive;

    [Header("Screen")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TextMeshProUGUI resText;
    [SerializeField] Resolution[] resolutionList; // x = width, y = height
    private int resId;
    private bool boolFullScreen;
    private CameraManager cameraManager;

    [Header("Audio")]
    [SerializeField] private UnityEvent playMenuMusic;
    private AudioSlider audioSlider;


    private void Awake()
    {
        resolutionList = UnityEngine.Screen.resolutions;
        GetScreenValue();
        isPlayerActive.SetValue(false);

        audioSlider = GetComponent<AudioSlider>();
        audioSlider.LoadSliderValue();
        
        playMenuMusic.Invoke();
        SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);

    }

    #region Main
    public void StartGame()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameSceneName));
        SceneManager.UnloadSceneAsync(mainMenuSceneName);
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

    void GetScreenValue()
    {
        if (!PlayerPrefs.HasKey("FullScreen")) PlayerPrefs.SetInt("FullScreen", 1);
        boolFullScreen = PlayerPrefs.GetInt("FullScreen") > 0 ? true : false;
        fullscreenToggle.isOn = boolFullScreen;

        if (!PlayerPrefs.HasKey("ResolutionId")) PlayerPrefs.SetInt("ResolutionId", resolutionList.Length -1);
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
        if(resId < 0) resId =  0;

        resText.text = $"{resolutionList[resId].width} x {resolutionList[resId].height}";
    }

    public void IncreaseRes()
    {
        resId++;
        if (resId >= resolutionList.Length) resId = resolutionList.Length - 1;

        resText.text = $"{resolutionList[resId].width} x {resolutionList[resId].height}";
    }
}
