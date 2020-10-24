using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MFireArrow : MonoBehaviourPunCallbacks
{
    public GameObject Canvas;

    public Animator bow;

    public PauseMenu menu;

    public MWeaponSelect weapon;

    public float charge;

    public float minCharge = 25f;

    public float maxCharge = 100f;

    public float coolDown = 0.1f;

    public float coolDownTimer = 0f;

    public GameObject Camera;

    public float chargeRate = 33f;

    public bool isClicked = false;

    public Transform spawn;
    public Rigidbody arrowObj;
    public Rigidbody eArrowObj;

    void Start()
    {
        Canvas = GameObject.Find("Canvas");
        menu = Canvas.GetComponent<PauseMenu>();
        charge = minCharge;
    }

    public void Update()
    {
        if (!photonView.IsMine) return;

        if (!menu.GameIsPaused)
        {
            bow.SetFloat("Charge", charge);

            if (Input.GetMouseButtonDown(0) && coolDownTimer >= coolDown)
            {
                isClicked = true;
            }
            if (isClicked)
            {
                if (charge < maxCharge)
                {
                    charge += Time.deltaTime * chargeRate;
                }
            }
            if (coolDownTimer < coolDown)
            {
                coolDownTimer += Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0) && coolDownTimer >= coolDown && isClicked)
            {
                isClicked = false;
                coolDownTimer = 0;
                if (weapon.equippedWeapon == "normalArrow")
                {
                    Rigidbody arrow = Instantiate(arrowObj, spawn.transform.position, spawn.transform.rotation) as Rigidbody;
                    arrow.AddForce(Camera.transform.forward * charge, ForceMode.Impulse);
                }
                else if (weapon.equippedWeapon == "explosiveArrow")
                {
                    Rigidbody eArrow = Instantiate(eArrowObj, spawn.transform.position, spawn.transform.rotation) as Rigidbody;
                    eArrow.AddForce(Camera.transform.forward * charge, ForceMode.Impulse);
                }
                charge = minCharge;
            }
        }
    }
}
