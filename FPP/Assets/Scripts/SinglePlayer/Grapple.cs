using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public GameObject Canvas;

    public PauseMenu menu;

    public WeaponSelect weapon;

    private LineRenderer lr;

    public float grappleCoolDown;

    private Vector3 grapplePoint;

    public LayerMask grappleable;

    private SpringJoint joint;

    public Transform firePoint;
    public Transform camera;

    public GameObject player;

    public float maxDistance = 100f;

    void Awake()
    {
        Canvas = GameObject.Find("Canvas");
        menu = Canvas.GetComponent<PauseMenu>();
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!menu.GameIsPaused)
        {
            if (grappleCoolDown < 0.5f)
            {
                grappleCoolDown += Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (grappleCoolDown >= 0.5f)
                {
                    StopGrapple();
                    StartGrapple();
                    grappleCoolDown = 0f;
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                StopGrapple();
            }
        }
        else
        {
            StopGrapple();
        }
        
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, grappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.transform.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 10f;
            joint.damper = 4f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, firePoint.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint); 
    }

}
