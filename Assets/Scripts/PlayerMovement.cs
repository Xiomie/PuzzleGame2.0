using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 10f;
    public bool isGrounded;

    public float mouseSensitivity = 100f;
    public Transform playerBody;

    public float maxPickupDistance = 2f;
    public float pickupSpeed = 5f;
    public float throwForce = 10f;
    public float pickupDistance = 2;

    public GameObject projectilePrefab; // Prefab of the projectile to shoot
    public Transform spawnPoint;        // Transform where the projectile will spawn
    public float fireRate = 0.5f;       // Time between shots
    private float nextFireTime = 0f;    // Time of the next shot
    public float projectileForce = 10f; // Force to apply to the projectile

    private bool isHoldingObject;
    private GameObject heldObject;

    public ParticleSystem lightning;
    private bool isLightningActive = false;

    float xRotation = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        isGrounded = CheckGrounded();
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.Self);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement *= runSpeed / moveSpeed;
            transform.Translate(movement, Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (!PauseMenu.isPaused)
        {
            if (Input.GetMouseButtonDown(1) && CanShoot())
            {
                ShootProjectile();
                nextFireTime = Time.time + fireRate;
            }
        }

        // Toggle lightning particle system
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleLightning();
        }
        // Rotate player based on mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);


        // Handle object pickup and drop
        if (Input.GetMouseButton(0))
        {
            if (!isHoldingObject)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxPickupDistance))
                {
                    if (hit.collider.CompareTag("Pickupable"))
                    {
                        heldObject = hit.collider.gameObject;
                        isHoldingObject = true;
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
            }
            else
            {
                Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * pickupDistance;
                heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, targetPosition, pickupSpeed * Time.deltaTime);
            }
        }
        if (Input.GetMouseButtonUp(0) && isHoldingObject)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject = null;
            isHoldingObject = false;
        }

        // Handle object throw
        if (Input.GetKeyDown(KeyCode.E) && isHoldingObject)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
            heldObject = null;
            isHoldingObject = false;
        }
        if (Input.GetMouseButtonDown(1) && !isLightningActive && CanShoot())
        {
            // Start shooting lightning
            lightning.transform.position = spawnPoint.position;
            lightning.transform.rotation = spawnPoint.rotation;
            lightning.Play();
            isLightningActive = true;
            nextFireTime = Time.time + fireRate;
        }
        else if (Input.GetMouseButtonUp(1) && isLightningActive)
        {
            // Stop shooting lightning
            lightning.Stop();
            isLightningActive = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isHoldingObject)
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject = null;
                isHoldingObject = false;
            }

            // Handle object throw
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isHoldingObject)
                {
                    heldObject.GetComponent<Rigidbody>().isKinematic = false;
                    heldObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
                    heldObject = null;
                    isHoldingObject = false;
                }
            }
        }
        void ShootProjectile()
        {
            if (projectilePrefab == null || spawnPoint == null) return;

            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
            if (projectileRB != null)
            {
                projectileRB.AddForce(spawnPoint.forward * projectileForce, ForceMode.Impulse);
            }

            // Play the lightning particle effect when shooting
            if (lightning != null)
            {
                ParticleSystem newLightning = Instantiate(lightning, spawnPoint.position, spawnPoint.rotation);
                newLightning.Play();
                // Destroy the lightning particle system after some time
                Destroy(newLightning.gameObject, newLightning.main.duration);
            }

            // Optional: Destroy the projectile after some time
            Destroy(projectile, 5f);
        }
         bool CheckGrounded()
        {
            float extraHeight = 0.1f;
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + new Vector3(0, GetComponent<Collider>().bounds.extents.y, 0);

            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, GetComponent<Collider>().bounds.extents.y + extraHeight))
            {
                return true;
            }
            return false;
        }
        void ToggleLightning()
        {
            if (lightning != null)
            {
                isLightningActive = !isLightningActive;
                if (isLightningActive)
                {
                    lightning.Play();
                }
                else
                {
                    lightning.Stop();
                }
            }
        }

        bool CanShoot()
        {
            // Check if the current time is after the next fire time
            return Time.time >= nextFireTime;
        }
    }
    }