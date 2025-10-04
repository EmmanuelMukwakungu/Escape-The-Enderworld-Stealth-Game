using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public GameObject PauseMenuScreen;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        PauseMenuScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}