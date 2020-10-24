using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSpin : MonoBehaviour
{

    public Animator anim;

    public string fan;

    void Start()
    {
        switch (fan)
        {
            case "start":
                anim.SetTrigger("Start");
                break;
            case "end":
                anim.SetTrigger("End");
                break;
            case "left":
                anim.SetTrigger("Left");
                break;
            case "right":
                anim.SetTrigger("Right");
                break;
        }
    }

}
