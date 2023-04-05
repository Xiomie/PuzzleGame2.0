using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBox : MonoBehaviour
{
     public GameObject door;          // Reference to the door GameObject
    public Animator doorAnimator;    // Reference to the door Animator
    public string openDoorAnimation; // The name of the door opening animation
    public ParticleSystem disableEffect;

    private bool isDisabled = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lightning") && !isDisabled)
        {
            DisableElectricBox();
            Destroy(door);
        }
    }

    void DisableElectricBox()
    {
        isDisabled = true;

        if (disableEffect != null)
        {
            disableEffect.Play();
        }
    }

    void DestroyDoor()
    {
        if (door != null)
        {
            Destroy(door);
        }
    }
}
