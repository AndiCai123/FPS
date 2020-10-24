using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject door;

    public GameObject[] enemies;

    public bool isAllDead = false;

    public bool allDead = false;

    void Update()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (i == 0) allDead = true;
            if (enemies[i] != null) allDead = false;
            if (i == enemies.Length - 1 && allDead) isAllDead = true;
        }

        if (isAllDead)
        {
            door.SetActive(false);
        }
    }
}
