using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class GameManager : MonoBehaviour
{
    public string player_prefab;

    public Transform[] spawnPoints;

    
    public Transform[] Zone1Spawn;
    public Transform[] Zone2Spawn;
    public Transform[] Zone3Spawn;
    public Transform[] Zone4Spawn;
    

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Transform t_spawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        PhotonNetwork.Instantiate(player_prefab, t_spawn.position, t_spawn.rotation);
    }
}
