using HelperScripts.EventSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    private int score;

    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] EventScriptable onPiecePlaced;

    [SerializeField] private UnityEvent playGameOver, StopSound, playMainMusic, playMenuMusic;
    [SerializeField] private BoolVariable isPlayerActive;
    [SerializeField] private FloatVariable happiness;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject ghost;

    [SerializeField] private GameObject Tuto;
    [SerializeField] private GameObject Tuto2;

    private bool playMusicOnce;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        isPlayerActive.OnValueChanged.AddListener(StartGame);
        Grid3DManager.Instance.onBalanceBroken.AddListener(GameOver);
        onPiecePlaced.AddListener(IncreaseScore);

        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            isPlayerActive.SetValue(true);
            playMainMusic.Invoke();
        }
        else
        {
            ui.SetActive(false);
            playMenuMusic.Invoke();
        }


            Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        if (isPlayerActive && PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            Instantiate(Tuto, this.transform);
            isPlayerActive.SetValue(false);
        }
        else
        {
            ui.SetActive(isPlayerActive);

            if (!playMusicOnce)
            {
                playMusicOnce = true;
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu")) playMenuMusic.Invoke();
                else playMainMusic.Invoke();
            }
        }
    }

    public void PauseGame()
    {
        isPlayerActive.SetValue(false);
        ghost.SetActive(isPlayerActive);
        ui.SetActive(isPlayerActive);
        pauseMenu.SetActive(true);
        //Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPlayerActive.SetValue(true);
        ghost.SetActive(isPlayerActive);
        ui.SetActive(isPlayerActive);
    }

    void GameOver()
    {
        StartCoroutine(endgame());
    }

    IEnumerator endgame() 
    {
        float animTime = Grid3DManager.Instance.GetComponent<TowerLeaningFeedback>().ShaderAnimTime + 0.37f ;
        yield return new WaitForSeconds(animTime);
        StopSound.Invoke();
        isPlayerActive.SetValue(false);
        ghost.SetActive(isPlayerActive);
        yield return new WaitForSeconds(5 - animTime);
        playGameOver.Invoke();
        var go = Instantiate(gameOverScreen);
        go.SetScore(score);
        go.SetHappiness(happiness);
    }

    private void OnDestroy()
    {
        Grid3DManager.Instance.onBalanceBroken.RemoveListener(GameOver);
        onPiecePlaced.RemoveListener(IncreaseScore);
    }

    private void IncreaseScore()
    {
        score++;
    }

    public void EndTuto()
    {
        isPlayerActive.SetValue(true);
    }
}
