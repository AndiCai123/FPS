using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopExploding : MonoBehaviour
{
    public float timer = 0f;

    public float time;

    public GameObject effect;

    void Update()
    {
        if (timer < time)
        {
            timer += Time.deltaTime;
        }
        if (timer >= time)
        {
            Destroy(effect);
        }
    }
}
