using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    public string equippedWeapon = "noramlArrow";

    public GameObject firework;

    void Update()
    {

        if (Input.GetKeyDown("q"))
        {
            if (equippedWeapon == "explosiveArrow")
            {
                equippedWeapon = "normalArrow";
                firework.SetActive(false);
            }
            else
            {
                equippedWeapon = "explosiveArrow";
                firework.SetActive(true);
            }
        }
    }
}
