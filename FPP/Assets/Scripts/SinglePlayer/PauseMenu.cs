using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;

    public GameObject SettingsMenu;

    public bool onSettings = false;

    public GameObject UI;

    public GameObject PauseUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        UI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        UI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadHub()
    {
        SceneManager.LoadScene("Hub");
        Time.timeScale = 1f;
    }

    public void LoadMultiplayer()
    {
        Debug.Log("Loading");
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        if (!onSettings)
        {
            SettingsMenu.SetActive(true);
            PauseUI.SetActive(false);
            onSettings = true;
        }
        else
        {
            SettingsMenu.SetActive(false);
            PauseUI.SetActive(true);
            onSettings = false;
        }
    }
}
