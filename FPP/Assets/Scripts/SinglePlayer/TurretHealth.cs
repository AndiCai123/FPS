using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{

    public GameObject explosionEffect;

    public float radius = 5f;

    public Transform explosionPosition;

    public Transform smaller;

    public GameObject smallExplosion;

    public int health = 3;

    //bool isDead = false;

    public GameObject Enemy;

    void Start()
    {
        
    }

    void Update()
    {
        if(health <= 0)
        {
            Die();
        }        
    }

    void Die()
    {
        //isDead = true;
        Destroy(Enemy);
        Instantiate(explosionEffect, explosionPosition.position, explosionPosition.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Arrow") || nearbyObject.CompareTag("ExplosiveArrow"))
            {
                Destroy(nearbyObject.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            health--;
            Instantiate(smallExplosion, smaller.position, smaller.rotation);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (other.gameObject.CompareTag("Explosion"))
        {
            health--;
        }
    }
}
