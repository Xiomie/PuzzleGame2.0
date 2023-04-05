using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class SecurityCamera : MonoBehaviour
{
    public float viewAngle = 90f;
    public float viewDistance = 10f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public ParticleSystem disabledEffect;   // Particle system to play when camera is disabled

    public Transform holdingPosition;       // Assign the HoldingPosition GameObject here
    public GameObject loseScreen;
    private Transform player;
    private bool isDisabled = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        loseScreen.SetActive(false);
    }

    void Update()
    {
        if (!isDisabled) // Only check for players if camera is not disabled
        {
            Vector3 directionToPlayer = player.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer < viewAngle / 2f && directionToPlayer.magnitude < viewDistance)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance, obstacleMask))
                {
                    Debug.DrawRay(transform.position, directionToPlayer * hit.distance, Color.red);
                }
                else
                {
                    Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.green);
                    Debug.Log("Spotted!");
                    OnPlayerSpotted();

                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickupable")) // Check if the object is pickable
        {
            other.transform.SetParent(holdingPosition); // Set the object as a child of HoldingPosition
            other.transform.localPosition = Vector3.zero; // Place the object in front of the camera
        }
        else if (other.CompareTag("Lightning")) // Check if camera was hit by a lightning particle system
        {
            isDisabled = true;
            if (disabledEffect != null) disabledEffect.Play(); // Play the disabled effect if it exists
            Debug.Log("Camera disabled!");
        }
    }

    public void OnPlayerSpotted()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.DrawWireSphere(transform.position + transform.forward * viewDistance, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0f, viewAngle / 2f, 0f) * transform.forward * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0f, -viewAngle / 2f, 0f) * transform.forward * viewDistance);
    }
}





