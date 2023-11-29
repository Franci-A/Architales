using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    private int score;


    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] EventScriptable onPiecePlaced;

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
        AudioManager.instance.StartMusic();
    }


    void GameOver()
    {
        StartCoroutine(endgame());
    }

    IEnumerator endgame() {
        yield return new WaitForSeconds(5);
        var go = Instantiate(gameOverScreen);
        go.SetScore(score);
    }

    private void OnDestroy()
    {
        Grid3DManager.Instance.onBalanceBroken.RemoveListener(GameOver);
    }

    private void IncreaseScore()
    {
        score++;
    }
}
