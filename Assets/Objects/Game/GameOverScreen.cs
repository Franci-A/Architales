using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    public void Restart() 
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
       SceneManager.LoadScene(0);
    }

    public void DestroyTower()
    {
        Grid3DManager.Instance.DestroyTower();
    }
}
