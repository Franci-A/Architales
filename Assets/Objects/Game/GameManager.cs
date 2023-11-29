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
        AudioManager.instance.StartMusic();
    }


    void GameOver()
    {
        StartCoroutine(endgame());
    }

    IEnumerator endgame() {
        yield return new WaitForSeconds(5);
        Instantiate(gameOverScreen);
    }

    private void OnDestroy()
    {
        Grid3DManager.Instance.onBalanceBroken.RemoveListener(GameOver);
    }
}
