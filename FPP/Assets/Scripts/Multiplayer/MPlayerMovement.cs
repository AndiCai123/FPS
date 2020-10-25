using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MPlayerMovement : MonoBehaviourPunCallbacks
{
    public GameObject Canvas;

    public PauseMenu menu;

    private Vector3 weaponOrigin;
    private Vector3 targetBobPosition;

    public Camera normalCam;

    public bool isAiming;

    public GameObject screenFlash;
    public float screenFlashTimer = 0.5f;

    public GameObject playerModel1;
    public GameObject playerModel2;
    public GameObject head;

    public int maxHealth;
    private int currentHealth;

    private Transform healthBar;
    private Text ammo;

    private GameManager manager;

    private Weapon weaponScript;

    public Transform playerCam;
    public Transform orientation;
    public Transform weapon;

    public GameObject cameraParent;

    private Rigidbody rb;

    private float xRotation;
    public float sensitivity = 50f;
    public float hipSens;
    private float sensMultiplier = 1f;
    
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    private Vector3 crouchScale = new Vector3(1, 0.8f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    public bool isWDown;
    private bool isADown;
    private bool isSDown;
    private bool isDDown;

    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    private bool doubleJumped = false;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded = true;
    public float doubleJumpForce = 400f;
    public float dashForce = 20f;


    private float movementCounter;
    private float idleCounter;

    float x, y;
    bool jumping, sprinting, crouching;

    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    void Awake()
    {
        hipSens = sensitivity;

        Canvas = GameObject.Find("Canvas");
        menu = Canvas.GetComponent<PauseMenu>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (!photonView.IsMine)
        {
            playerModel1.layer = 20;
            playerModel2.layer = 20;
            head.layer = 20;
        }
        screenFlash = GameObject.Find("Canvas/HUD/ScreenFlash");

        manager = GameObject.Find("Manager").GetComponent<GameManager>();
        weaponScript = GetComponent<Weapon>();


        if (photonView.IsMine)
        {
            healthBar = GameObject.Find("HUD/Health/Bar").transform;
            ammo = GameObject.Find("HUD/Ammo/Text").GetComponent<Text>();
            RefreshHealthBar();
            weaponScript.RefreshAmmo(ammo);
        }

        currentHealth = maxHealth;

        cameraParent.SetActive(photonView.IsMine);

        if (!photonView.IsMine)
        {
            gameObject.layer = 20;
        }

        weaponOrigin = weapon.localPosition;

        playerScale = transform.localScale;
    }


    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        Movement();
        //bool aim = Input.GetMouseButton(1);

        //isAiming = aim;

        //weaponScript.Aim(isAiming);

        //if (isAiming)
        //{
        //}        
        //if (!isAiming)
        //{
        //    normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, 90, Time.deltaTime * 8f);
        //}
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (weaponScript.isSpawned)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isAiming = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                isAiming = false;
            }
        }

        if (screenFlashTimer < 0.5f)
        {
            screenFlashTimer += Time.deltaTime;
        }

        if (screenFlashTimer >= 0.5f)
        {
            if (screenFlash == null)
            {
                screenFlash = GameObject.Find("Canvas/HUD/ScreenFlash");
            }
            screenFlash.SetActive(false);
        }

        if (x == 0 && y == 0)
        {
            if(!weaponScript.isADS) HeadBob(idleCounter, 0.025f, 0.025f);
            else HeadBob(idleCounter, 0.0001f, 0.0001f);
            idleCounter += Time.deltaTime;
            weapon.localPosition = Vector3.Lerp(weapon.localPosition, targetBobPosition, Time.deltaTime * 2f);
        }
        else
        {
            if (!weaponScript.isADS) HeadBob(movementCounter, 0.05f, 0.05f);
            else HeadBob(movementCounter, 0.0001f, 0.0001f);
            movementCounter += Time.deltaTime * 3f;
            weapon.localPosition = Vector3.Lerp(weapon.localPosition, targetBobPosition, Time.deltaTime * 8f);
        }

        if (isAiming)
        {
            sensitivity = weaponScript.currentGunData.adssens;
        }
        else
        {
            sensitivity = hipSens;
        }


        RefreshHealthBar();
        weaponScript.RefreshAmmo(ammo);

        MyInput();

        Look();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown("w")) isWDown = true;
        if (Input.GetKeyDown("a")) isADown = true;
        if (Input.GetKeyDown("s")) isSDown = true;
        if (Input.GetKeyDown("d")) isDDown = true;

        if (Input.GetKeyUp("w")) isWDown = false;
        if (Input.GetKeyUp("a")) isADown = false;
        if (Input.GetKeyUp("s")) isSDown = false;
        if (Input.GetKeyUp("d")) isDDown = false;

        if (Input.GetKeyDown("space") && !isGrounded)
        {
            if (!doubleJumped)
            {
                if (isWDown)
                {
                    if (rb.velocity.y > 0) rb.AddForce(playerCam.transform.forward * dashForce, ForceMode.Impulse);
                    else rb.AddForce(playerCam.transform.forward * dashForce, ForceMode.Impulse);
                }
                else if (isADown)
                {
                    if (rb.velocity.y > 0) rb.AddForce(-playerCam.transform.right * dashForce, ForceMode.Impulse);
                    else rb.AddForce(-playerCam.transform.right * dashForce, ForceMode.Impulse);
                }
                else if (isSDown)
                {
                    if (rb.velocity.y > 0) rb.AddForce(-playerCam.transform.forward * dashForce, ForceMode.Impulse);
                    else rb.AddForce(-playerCam.transform.forward * dashForce, ForceMode.Impulse);
                }
                else if (isDDown)
                {
                    if (rb.velocity.y > 0) rb.AddForce(playerCam.transform.right * dashForce, ForceMode.Impulse);
                    else rb.AddForce(playerCam.transform.right * dashForce, ForceMode.Impulse);
                }
                else
                {
                    if (rb.velocity.y > 0) rb.AddForce(transform.up * doubleJumpForce, ForceMode.Impulse);
                    else rb.AddForce(transform.up * doubleJumpForce, ForceMode.Impulse);
                }
                doubleJumped = true;
            }
        }

        if (isGrounded)
        {
            doubleJumped = false;
        }
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        CounterMovement(x, y, mag);

        if (readyToJump && jumping) Jump();

        float maxSpeed = this.maxSpeed;

        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        float multiplier = 1f, multiplierV = 1f;

        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        if (grounded && crouching) multiplierV = 0f;

        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredX;

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    void RefreshHealthBar()
    {
        float t_health_ratio = (float)currentHealth / (float)maxHealth;
        healthBar.localScale = new Vector3(t_health_ratio * 1.5f, 0.25f, 1);
    }

    void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
    {
        targetBobPosition = weaponOrigin + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity, 0);
    }

    private void OnCollisionStay(Collision other)
    {
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }

    public void TakeDamage(int damage)
    {
        if (photonView.IsMine)
        {
            currentHealth -= damage;
            RefreshHealthBar();

            screenFlash.SetActive(true);
            screenFlashTimer = 0f;

            if (currentHealth <= 0)
            {
                manager.Spawn();
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("You Died");
            }
        }
    }

}