using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI happinessText;
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
        scoreText.text = $"{_score.ToString()}";
    }
    
    public void SetHappiness(float _score)
    {
        happinessText.text = $"{_score.ToString()}";
    }
}
