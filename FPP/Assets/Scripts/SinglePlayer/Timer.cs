using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timer;

    public float RoundedTimer;

    public bool finished;

    public Text timerText;

    public Text finishedTimerText;

    public Rigidbody rb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            timer += Time.deltaTime;
            timerText.text = RoundedTimer.ToString();
        }
        else
        {
            finishedTimerText.text = timerText.text;
        }
        RoundedTimer = ((int)(timer * 100)) / 100f;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            finished = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
