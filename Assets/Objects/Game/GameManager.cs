using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.Events;
using UnityEditor.PackageManager;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    private int score;


    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] EventScriptable onPiecePlaced;

    [SerializeField] private UnityEvent playMusic, playGameOver;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Grid3DManager.Instance.onBalanceBroken.AddListener(GameOver);
        onPiecePlaced.AddListener(IncreaseScore);

        playMusic.Invoke();
    }



    void GameOver()
    {
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
