using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume() // funkcja wznowienia gry
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() // funkcja pauzy
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); //³adowanie menu
        //Debug.Log("Menu");
    }

    public void LoadOptions()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("OptionsMenu"); //³adowanie menu opcji
    }

    public void QuitGame()
    {
        Debug.Log("Zamykanie gry");
        Application.Quit();
    }


}
