using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Tools")]
    public GameObject pickaxeObject;

    [Header("Animation")]
    public Animator animator;

    [Header("Digging")]
    public Pickaxe pickaxe;
    public int digDamage = 1;

    [Header("Shooting")]
    public GameObject pistolObject;

    [Header("Movement")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 6.5f;

    [Header("Camera")]
    public Transform cameraTransform;
    public float mouseSensitivity = 90f;
    public float maxLookAngle = 50f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public GameObject muzzleObject;
    public float bulletSpeed = 30f;
    public float fireCooldown = 0.2f;

    CharacterController controller;
    PlayerInventory inventory;

    bool pickaxeEquipped;
    float xRotation;
    float nextFireTime;

    [Header("Audio")]
    public AudioSource gunAudioSource;
    public AudioClip gunShotClip;
    public float gunShotVolume = 1f;

    public AudioSource pickaxeAudioSource;
    public AudioClip pickaxeDigClip;
    public float digVolume = 1f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inventory = GetComponent<PlayerInventory>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        pickaxeEquipped = true;
        pickaxeObject.SetActive(true);
        pistolObject.SetActive(false);

        animator.SetBool("GunMode", false);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleToolSwitch();
        Move();
        Look();

        if (pickaxeEquipped)
            Dig();
        else
            Shoot();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude > 1f)
            move.Normalize();

        bool isMoving = move.magnitude > 0.05f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        float currentSpeed;
        if (isRunning)
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;

        Vector3 horizontalMove = move * currentSpeed;

        Vector3 finalMove = horizontalMove;
        controller.Move(finalMove * Time.deltaTime);

        float animSpeed = 0f;
        if (isMoving)
            animSpeed = 0.5f;
        if (isRunning)
            animSpeed = 1f;

        animator.SetFloat("Speed", animSpeed);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;

            if (gunAudioSource != null && gunShotClip != null)
                gunAudioSource.PlayOneShot(gunShotClip, gunShotVolume);

            Ray cameraRay = new Ray(cameraTransform.position, cameraTransform.forward);

            Vector3 targetPoint;

            if (Physics.Raycast(cameraRay, out RaycastHit hit, 100f))
                targetPoint = hit.point;
            else
                targetPoint = cameraTransform.position + cameraTransform.forward * 100f;

            Vector3 shotDirection = (targetPoint - muzzleObject.transform.position).normalized;

            GameObject bulletObj = Instantiate(
                bulletPrefab,
                muzzleObject.transform.position,
                Quaternion.LookRotation(shotDirection)
            );

            Physics.IgnoreCollision(
                bulletObj.GetComponent<Collider>(),
                GetComponent<CharacterController>()
            );

            bulletObj.GetComponent<Rigidbody>().velocity = shotDirection * bulletSpeed;
        }
    }

    void Dig()
    {
        if (Input.GetMouseButtonDown(0))
        {                
            animator.SetTrigger("Dig");

            if (pickaxe != null && pickaxe.currentMineral != null && pickaxeAudioSource != null && pickaxeDigClip != null)
                pickaxeAudioSource.PlayOneShot(pickaxeDigClip, digVolume);
                pickaxe.currentMineral.TakeDamage(digDamage, inventory);
        }
    }

    void HandleToolSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pickaxeEquipped = true;
            pickaxeObject.SetActive(true);
            pistolObject.SetActive(false);

            if (pickaxe != null)
                pickaxe.currentMineral = null;

            animator.SetBool("GunMode", false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pickaxeEquipped = false;
            pickaxeObject.SetActive(false);
            pistolObject.SetActive(true);

            animator.SetBool("GunMode", true);
        }
    }
}