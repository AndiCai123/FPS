using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Launcher launcher;

    public void Start()
    {
        launcher = GetComponent<Launcher>();
    }

    public void JoinMatch()
    {
        launcher.Join();
        launcher.JoinButton.SetActive(false);
        launcher.CreateButton.SetActive(false);
        launcher.QuitButton.SetActive(false);
        launcher.Loading.SetActive(true);
    }

    public void CreateMatch()
    {
        launcher.Create();
        launcher.JoinButton.SetActive(false);
        launcher.CreateButton.SetActive(false);
        launcher.QuitButton.SetActive(false);
        launcher.Loading.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
