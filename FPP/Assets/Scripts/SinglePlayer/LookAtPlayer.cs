using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{

    public TurretManager manager;

    public GameObject head;

    public GameObject FirePoint;

    // Update is called once per frame
    void Update()
    {
        if (manager.inRange)
        {
            head.transform.LookAt(manager.player.transform);
            FirePoint.transform.LookAt(manager.player.transform);
        }
    }
}
