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
    }

    public void CreateMatch()
    {
        launcher.Create();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
