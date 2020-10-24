using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawn;
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -300)
        {
            transform.position = respawn.position;
        }
    }
}
