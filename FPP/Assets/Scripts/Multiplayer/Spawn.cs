using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public bool spawning;
    public int primary = 3;
    public int secondary = 0;
    public int zone;
    public GameObject deployScreen;

    // Update is called once per frame

    public void Init()
    {
        deployScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SpawnPlayer(int i)
    {
        deployScreen.SetActive(false);
        spawning = true;
        zone = i;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetPrimary(int i)
    {
        primary = i;
    }

    public void SetSecondary(int i)
    {
        secondary = i;
    }
}
