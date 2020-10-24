using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown rezDropDown;

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        rezDropDown.ClearOptions();

        List<string> options = new List<string>();

        int Index = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                Index = i;
            }
        }

        rezDropDown.AddOptions(options);
        rezDropDown.value = Index;
        rezDropDown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetRez(int RezIndex)
    {
        Resolution rezs = resolutions[RezIndex];
        Screen.SetResolution(rezs.width, rezs.height, Screen.fullScreen);
    }
}
