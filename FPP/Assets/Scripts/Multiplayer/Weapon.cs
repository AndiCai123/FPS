using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class Weapon : MonoBehaviourPunCallbacks
{
    //public GameObject pistol;

    //public Transform Hip;

    //public float smooth;

    public LineRenderer tracer;

    public Spawn spawn;

    public GameObject deployScreen;

    public MPlayerMovement movement;

    public Rigidbody rb;

    private bool isReloading = false;

    public bool isADS;

    public GameObject hitMarker;
    public GameObject HSMarker;

    public GameManager manager;

    public GameObject mySelf;

    public bool isFired;

    private float tracerTime = 0.1f;

    public Transform Anchor;

    public float timer;

    public float currentBloom;

    private float currentCoolDown;

    public Transform barrel;

    public Gun[] loadout;

    [HideInInspector] public Gun currentGunData;

    public Transform weaponParent;

    public Transform t_spawn;

    private int currentIndex;

    public GameObject bulletholePrefab;

    public LayerMask canBeShot;

    private GameObject currentWeapon;

    float hitMarkerTimer = 0.5f;
    float HSMarkerTimer = 0.5f;

    public void Start()
    {
        hitMarker = GameObject.Find("Canvas/HUD/HitMarker");
        HSMarker = GameObject.Find("Canvas/HUD/HeadShotHitMarker");
        deployScreen = GameObject.Find("Canvas/Deploy");
        spawn = GameObject.Find("Canvas").GetComponent<Spawn>();
        movement = GetComponent<MPlayerMovement>();
        rb = GetComponent<Rigidbody>();
        if (photonView.IsMine)
        {
            spawn.Init();
            Debug.Log("spawned");
        }
        //movement = GetComponent<MPlayerMovement>();
        //tracer.enabled = false;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        movement = GetComponent<MPlayerMovement>();

        Debug.Log(spawn.primary);

        if (hitMarkerTimer < 0.5f)
        {
            hitMarkerTimer += Time.deltaTime;
        }

        if (spawn.spawning)
        {
            Spawn(spawn.zone);
        }

        if(hitMarkerTimer >= 0.5f)
        {
            if (hitMarker == null)
            {
                hitMarker = GameObject.Find("Canvas/HUD/HitMarker");
            }
            hitMarker.SetActive(false);
        }        
        
        if (HSMarkerTimer < 0.5f)
        {
            HSMarkerTimer += Time.deltaTime;
        }

        if(HSMarkerTimer >= 0.5f)
        {
            if (HSMarker == null)
            {
                HSMarker = GameObject.Find("Canvas/HUD/HeadShotHitMarker");
            }
            HSMarker.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.Alpha1)) { photonView.RPC("Equip", RpcTarget.All, spawn.primary); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { photonView.RPC("Equip", RpcTarget.All, spawn.secondary); }

        //if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);
        if (currentWeapon != null)
        {
            Anchor = currentWeapon.transform.Find("Anchor");
            //barrel = currentWeapon.transform.Find("Anchor/Resources/Barrel");
            //tracer = barrel.GetComponent<LineRenderer>();

            //Hip = currentWeapon.transform.Find("States/Hip");

            //currentWeapon.transform.rotation = Quaternion.Lerp(transform.rotation, Hip.rotation, Time.deltaTime/* * smooth*/);

            //Aim(Input.GetMouseButton(1));

            tracer.useWorldSpace = true;

            if (isADS)
            {
                if(currentBloom > loadout[currentIndex].adsbloom)
                {
                    currentBloom -= Time.deltaTime * (loadout[currentIndex].bloom - loadout[currentIndex].adsbloom) * (loadout[currentIndex].adsaccuracyspeed);
                    Debug.Log(currentBloom);
                }
                //movement.moveSpeed = movement.moveSpeed * loadout[currentIndex].adswalkspeed;
                movement.moveSpeed = loadout[currentIndex].adswalkspeed;
            }
            else
            {
                currentBloom = loadout[currentIndex].bloom;
                //movement.moveSpeed = movement.moveSpeed / loadout[currentIndex].adswalkspeed;
                movement.moveSpeed = loadout[currentIndex].walkspeed;
            }

            if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Reload(loadout[currentIndex].reloadtime));

            if (loadout[currentIndex].burst != 1)
            {

                if (Input.GetMouseButtonDown(0) && currentCoolDown <= 0 && !isReloading)
                {
                    if (loadout[currentIndex].FireBullet())
                    {
                        photonView.RPC("Shoot", RpcTarget.All);
                    }
                    //else StartCoroutine(Reload(loadout[currentIndex].reloadtime));

                    //Shoot();
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && currentCoolDown <= 0 && !isReloading)
                {
                    if (loadout[currentIndex].FireBullet())
                    {
                        photonView.RPC("Shoot", RpcTarget.All);
                    }
                    loadout[currentIndex].recoilaccuracy += Time.deltaTime * loadout[currentIndex].recoilmultiplier;
                    //else StartCoroutine(Reload(loadout[currentIndex].reloadtime));

                    //Shoot();
                }
                else if(!Input.GetMouseButton(0) && loadout[currentIndex].recoilaccuracy > 0)
                {
                    loadout[currentIndex].recoilaccuracy -= Time.deltaTime * loadout[currentIndex].recoilrecovery;
                }
            }
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
            currentWeapon.transform.rotation = Quaternion.Lerp(Anchor.rotation, weaponParent.rotation, Time.deltaTime * 5f/* * smooth*/);

            if (currentCoolDown > 0) currentCoolDown -= Time.deltaTime;
        }

        if (timer < tracerTime)
        {
            timer += Time.deltaTime;
        }

        if(timer >= tracerTime)
        {
            if (currentWeapon != null)
            {
                if (tracer.enabled)
                {
                    photonView.RPC("RemoveTracer", RpcTarget.All);
                }
            }
        }
        
    }

    public void RefreshAmmo(Text p_text)
    {
        int t_clip = loadout[currentIndex].GetClip();
        int t_stash = loadout[currentIndex].GetStash();

        p_text.text = t_clip.ToString() + " / " + t_stash.ToString();
    }

    /*
    [PunRPC]
    public void Deploy()
    {
        deployScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    */

    [PunRPC]
    public void Equip(int p_ind)
    {
        if (currentWeapon != null)
        {
            if(isReloading) StopCoroutine("Reload");
            Destroy(currentWeapon);
        }


        //Quaternion target_rotation = origin_rotation;

        currentIndex = p_ind;

        GameObject t_newWeapon = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        t_newWeapon.transform.localPosition = new Vector3(0,-0.25f,0);
        t_newWeapon.transform.localEulerAngles = Vector3.zero;
        currentWeapon = t_newWeapon;

        if (photonView.IsMine) ChangeLayerRecursively(t_newWeapon, 19);
        else ChangeLayerRecursively(t_newWeapon, 0);

        currentGunData = loadout[p_ind];
        barrel = currentWeapon.transform.Find("Anchor/Resources/Barrel");
        loadout[currentIndex].recoilaccuracy = 0;
        movement.moveSpeed = loadout[currentIndex].walkspeed;
    }

    public void Aim(bool isAiming)
    {
        if (!currentWeapon) return;

        Transform t_anchor = currentWeapon.transform.Find("Anchor");
        Transform t_ads = currentWeapon.transform.Find("States/ADS");
        Transform t_hip = currentWeapon.transform.Find("States/Hip");
        if (isAiming)
        {
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_ads.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            isADS = true;
        }
        else
        {
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            isADS = false;
        }
    }

    private void ChangeLayerRecursively(GameObject p_target, int p_layer)
    {
        p_target.layer = p_layer;
        foreach (Transform a in p_target.transform) ChangeLayerRecursively(a.gameObject, p_layer);
    }

    [PunRPC]
    public void Shoot()
    {
        Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;
        t_bloom += UnityEngine.Random.Range(-currentBloom + loadout[currentIndex].recoilaccuracy, currentBloom + loadout[currentIndex].recoilaccuracy) * t_spawn.up;
        t_bloom += UnityEngine.Random.Range(-currentBloom - loadout[currentIndex].recoilaccuracy / 2, currentBloom + loadout[currentIndex].recoilaccuracy / 2) * t_spawn.right;
        t_bloom -= t_spawn.position;
        t_bloom.Normalize();


        RaycastHit t_hit = new RaycastHit();
        if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, canBeShot))
        {
            tracer.SetPosition(0, barrel.position);
            tracer.SetPosition(1, t_hit.point);
            tracer.enabled = true;
            timer = 0;
            GameObject t_newHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
            t_newHole.transform.LookAt(t_hit.point + t_hit.normal);
            Destroy(t_newHole, 5f);

            if (photonView.IsMine)
            {
                if(t_hit.collider.gameObject.layer == 20)
                {
                    if (t_hit.collider.gameObject.CompareTag("Head"))
                    {
                        Debug.Log("headshot");
                        t_hit.collider.gameObject.transform.parent.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].headshotdamage);
                        HSMarker.SetActive(true);
                        HSMarkerTimer = 0f;
                    }
                    else
                    {
                        t_hit.collider.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage);
                        hitMarker.SetActive(true);
                        hitMarkerTimer = 0f;
                    }
                    t_newHole.transform.parent = t_hit.collider.gameObject.transform;

                }
            }
        }
        currentCoolDown = loadout[currentIndex].firerate;

        if (photonView.IsMine)
        {
            currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kickback;
        }
    }

    IEnumerator Reload(float p_wait)
    {
        isReloading = true;
        currentWeapon.SetActive(false);
        yield return new WaitForSeconds(p_wait);
        loadout[currentIndex].Reload();
        currentWeapon.SetActive(true);
        isReloading = false;
    }

    [PunRPC]
    void TakeDamage(int damage)
    {
        GetComponent<MPlayerMovement>().TakeDamage(damage);
    }

    [PunRPC]
    void RemoveTracer()
    {
        tracer.enabled = false;
    }


    public void Spawn(int i)
    {
        if (!photonView.IsMine) return;
        //Debug.Log("Spawned");
        //deployScreen = GameObject.Find("Canvas/Deploy");
        //deployScreen.SetActive(false);
        foreach (Gun a in loadout) a.Initialize();
        photonView.RPC("Equip", RpcTarget.All, spawn.primary);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        manager = GameObject.Find("Manager").GetComponent<GameManager>();
        if (!photonView.IsMine)
        {
            movement.playerModel1.layer = 20;
            movement.playerModel2.layer = 20;
            movement.head.layer = 20;
        }

        if (i == 1)
        {
            transform.position = manager.Zone1Spawn[UnityEngine.Random.Range(0, manager.Zone1Spawn.Length)].position;
        }
        if (i == 2)
        {
            transform.position = manager.Zone2Spawn[UnityEngine.Random.Range(0, manager.Zone2Spawn.Length)].position;
        }
        if (i == 3)
        {
            transform.position = manager.Zone3Spawn[UnityEngine.Random.Range(0, manager.Zone3Spawn.Length)].position;
        }
        if (i == 4)
        {
            transform.position = manager.Zone4Spawn[UnityEngine.Random.Range(0, manager.Zone4Spawn.Length)].position;
        }
        spawn.spawning = false;

    }
}
