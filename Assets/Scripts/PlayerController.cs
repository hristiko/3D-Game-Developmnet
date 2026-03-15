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
    //float yVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inventory = GetComponent<PlayerInventory>();
        animator = GetComponentInChildren<Animator>();

        pickaxeEquipped = true;
        pickaxeObject.SetActive(true);
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

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

            GameObject bulletObj = Instantiate(
                bulletPrefab,
                muzzleObject.transform.position,
                muzzleObject.transform.rotation
            );

            Physics.IgnoreCollision(
                bulletObj.GetComponent<Collider>(),
                GetComponent<CharacterController>()
            );

            bulletObj.GetComponent<Rigidbody>().velocity =
                muzzleObject.transform.forward * bulletSpeed;
        }
    }

    void Dig()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Dig");

            if (pickaxe != null && pickaxe.currentMineral != null)
                pickaxe.currentMineral.TakeDamage(digDamage, inventory);
        }
    }

    void HandleToolSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pickaxeEquipped = false;
            pickaxeObject.SetActive(false);

            if (pickaxe != null)
                pickaxe.currentMineral = null;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pickaxeEquipped = true;
            pickaxeObject.SetActive(true);
        }
    }
}