using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryDoor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pickupable"))
        {
            Destroy(gameObject);
        }
    }
}