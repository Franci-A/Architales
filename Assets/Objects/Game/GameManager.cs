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

    [SerializeField] private UnityEvent playMusic, playGameOver;
    [SerializeField] private BoolVariable isPlayerActive;
    [SerializeField] private GameObject ui;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            isPlayerActive.SetValue(true);
        }else
            ui.SetActive(false);
        isPlayerActive.OnValueChanged.AddListener(StartGame);

        Grid3DManager.Instance.onBalanceBroken.AddListener(GameOver);
        onPiecePlaced.AddListener(IncreaseScore);

        playMusic.Invoke();
    }

    public void StartGame()
    {
        ui.SetActive(isPlayerActive);
    }

    void GameOver()
    {
        isPlayerActive.SetValue(false);
        StartCoroutine(endgame());
    }

    IEnumerator endgame() {
        yield return new WaitForSeconds(5);
        playGameOver.Invoke();
        var go = Instantiate(gameOverScreen);
        go.SetScore(score);
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
}
