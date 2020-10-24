using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]

public class Gun : ScriptableObject
{
    public string name;
    public int damage;
    public int headshotdamage;
    public float recoilaccuracy;
    public float recoilmultiplier;
    public float recoilrecovery;
    public int burst;
    public int clipsize;
    public int ammo;
    public float walkspeed;
    public float adswalkspeed;
    public float adsbloom;
    public float adsaccuracyspeed;
    public float adssens;
    public float firerate;
    public float bloom;
    public float reloadtime;
    public float recoil;
    public float kickback;
    public float aimSpeed;
    [Range(0, 1)] public float mainFOV;
    [Range(0, 1)] public float weaponFOV;
    public GameObject prefab;

    private int stash;
    private int clip;

    public void Initialize()
    {
        stash = ammo;
        clip = clipsize;
    }

    public bool  FireBullet()
    {
        if (clip > 0)
        {
            clip -= 1;
            return true;
        }
        else return false;
    }

    public void Reload()
    {
        stash += clip;
        clip = Mathf.Min(clipsize, stash);
        stash -= clip;
    }

    public int GetStash() { return stash; }
    public int GetClip() { return clip; }
}
