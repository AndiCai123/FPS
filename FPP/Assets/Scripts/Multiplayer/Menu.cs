using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Launcher launcher;

    public void SinglePlayer()
    {
        SceneManager.LoadScene("Hub");
    }

    public void Multiplayer()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
