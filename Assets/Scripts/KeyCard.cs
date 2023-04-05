using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour
{
    public string keycardTag = "Keycard";
    public string prisonDoorTag = "PrisonDoor";
    public string doorOpenAnimationName = "DoorOpen"; // Name of the door opening animation

    private Animator doorAnimator;

    void Start()
    {
        GameObject door = GameObject.FindGameObjectWithTag(prisonDoorTag);
        if (door != null)
        {
            doorAnimator = door.GetComponent<Animator>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(keycardTag) && gameObject.CompareTag(prisonDoorTag))
        {
            if (doorAnimator != null)
            {
                doorAnimator.Play(doorOpenAnimationName);
            }
        }
    }
}