using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;
    public static GameManager Instance { get => instance; }


    [SerializeField] GameObject gameOverScreen;

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
    }


    void GameOver()
    {
        Instantiate(gameOverScreen);
    }
}
