using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    public GameObject rotateAnchor;

    public GameObject Player;

    public Transform destination;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(rotateAnchor.transform.position, Vector3.up, 10 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Player.transform.position = destination.position;
    }
}
