using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleShading : MonoBehaviour
{
    public GameObject highVolume;

    public GameObject lowVolume;

    public void toggle(bool isOn)
    {
        if (isOn)
        {
            highVolume.SetActive(true);
            lowVolume.SetActive(false);
        }
        else
        {
            lowVolume.SetActive(true);
            highVolume.SetActive(false);
        }
    }
}
