using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurretManager : MonoBehaviour
{

    public float aimOffset;

    public GameObject enemy;

    public GameObject player;

    public float fireRate = 1f;

    public float fireCoolDown;

    public float bulletSpeed = 50f;

    public float bloom = 3f;

    public Rigidbody bullet;

    public Transform firePoint;

    public bool inRange = false;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange)
        {
            enemy.transform.LookAt(player.transform);
        }

        if (fireCoolDown < fireRate)
        {
            fireCoolDown += Time.deltaTime;
        }

        if (inRange && fireCoolDown >= fireRate)
        {
            Rigidbody bulletRB = Instantiate(bullet, firePoint.position, firePoint.rotation) as Rigidbody;
            bulletRB.AddForce(new Vector3(player.transform.position.x - transform.position.x + Random.Range(-bloom, bloom), player.transform.position.y - transform.position.y + Random.Range(-bloom, bloom) - aimOffset, player.transform.position.z - transform.position.z + Random.Range(-bloom, bloom)) * bulletSpeed, ForceMode.Impulse);
            fireCoolDown = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
