using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MWallJump : MonoBehaviourPunCallbacks
{

    public Rigidbody rb;

    public bool walled = false;

    public MPlayerMovement movement;

    void Update()
    {
        if (!photonView.IsMine) return;

        if (walled)
        {
            if (Input.GetKeyDown("space"))
            {
                if (movement.isWDown)
                {
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                    if (rb.velocity.y > 0) rb.AddForce(movement.playerCam.transform.forward * movement.dashForce, ForceMode.Impulse);
                    else rb.AddForce(movement.playerCam.transform.forward * movement.dashForce, ForceMode.Impulse);
                }
                else
                {
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                }
                walled = false;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall") && !movement.isGrounded){
            rb.constraints = RigidbodyConstraints.FreezeAll;
            walled = true;
        }
    }
}
