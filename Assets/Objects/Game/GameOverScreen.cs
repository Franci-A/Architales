using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void Restart() 
    {
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
