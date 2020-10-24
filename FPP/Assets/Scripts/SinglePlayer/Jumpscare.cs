using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jumpscare : MonoBehaviour
{
    public static bool hasJumpscared;

    public GameObject MONKE;

    public float Timer = 0f;

    public bool activated = false;

    void Update()
    {
        if (!hasJumpscared)
        {
            if (activated)
            {
                Timer += Time.deltaTime;
            }
            if (Timer >= 2f)
            {
                MONKE.SetActive(false);
                hasJumpscared = false;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!hasJumpscared)
            {
                MONKE.SetActive(true);
                activated = true;
            }
        }
    }
}
