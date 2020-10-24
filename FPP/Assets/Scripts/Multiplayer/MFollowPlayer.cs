using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MFollowPlayer : MonoBehaviourPunCallbacks
{

    public Transform player;

    void Update()
    {
        if (!photonView.IsMine) return;

        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
