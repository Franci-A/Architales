using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    public void Restart() 
    {
        SceneManager.LoadScene(0);
    }

    public void BackToMenu()
    {
        Debug.Log("BackToMenu :D");
        //SceneManager.LoadScene(menu);
    }

    public void DestroyTower()
    {
        Grid3DManager.Instance.DestroyTower();
    }
}
