using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public GameObject explosionEffect;

    public float radius = 5f;

    public float force = 700f;

    public bool hasExploded = false;

    public GameObject EArrow;

    public SphereCollider explosion;

    public float Timer = 0f;


    void Start()
    {
        explosion.enabled = !explosion.enabled;
    }

    void Update()
    {
        if(Timer < 2f && hasExploded)
        {
            Timer += Time.deltaTime;
        }
        if(Timer >= 2f)
        {
            Destroy(EArrow);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!hasExploded)
        {
            explode();
            explosion.enabled = !explosion.enabled;
        }
    }
    public void explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Player"))
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, radius);
                }
            }
            else if (nearbyObject.CompareTag("Arrow") || nearbyObject.CompareTag("ExplosiveArrow"))
            {
                Destroy(nearbyObject.gameObject);
            }
        }

        hasExploded = true;
    }
}
