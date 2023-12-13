using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private UnityEvent playMainMusic;

    public void Restart() 
    {
        playMainMusic.Invoke();
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetScore(float _score)
    {
        scoreText.text = $"Pieces placed : {_score.ToString()}";
    }
}
