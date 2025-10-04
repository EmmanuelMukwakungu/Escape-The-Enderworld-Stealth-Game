using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    private void Update()
    {
        PressSpacebar();
    }

    public void PressSpacebar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Stealth Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
