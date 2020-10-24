﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject Health;

    public Slider healthSlider;

    public int health = 5;

    public bool isDead = false;

    public Transform hubSpawn;

    public Rigidbody rb;

    void Start()
    {
        Health = GameObject.Find("Health");
        healthSlider = Health.GetComponent<Slider>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
        healthSlider.value = health;
    }

    void Die()
    {
        isDead = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health--;
        }
    }
}
