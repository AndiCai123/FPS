using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody rb;

    void Update()
    {
        if (rb.velocity.x > 0 || rb.velocity.y > 0 || rb.velocity.z > 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
