using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    public GameObject loseScreen;

    private void Start()
    {
        loseScreen.SetActive(false);
    }

    public void OnPlayerSpotted()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}
