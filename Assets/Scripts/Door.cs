using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{


    public Animator animator; // The Animator component attached to the door

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object entering the trigger is the player
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if (animator != null)
        {
            animator.SetBool("Open", true);
        }
    }
}