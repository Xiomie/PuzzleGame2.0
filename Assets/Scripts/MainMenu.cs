using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void Game()
    {
        SceneManager.LoadScene("LevelOne");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
