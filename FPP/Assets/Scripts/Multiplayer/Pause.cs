using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public bool paused = false;

    private bool disconnecting = false;

    public GameObject pauseMenu;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (disconnecting) return;

        paused = !paused;

        pauseMenu.SetActive(paused);
        Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    
    public void Quit()
    {
        disconnecting = true;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MultiplayerMenu");
    }
}
