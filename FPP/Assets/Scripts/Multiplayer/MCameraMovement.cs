using UnityEngine;
using Photon.Pun;

public class MCameraMovement : MonoBehaviourPunCallbacks
{

    public Transform player;

    void Update()
    {
        //if (!photonView.IsMine) return;
        transform.position = player.transform.position;
    }
}