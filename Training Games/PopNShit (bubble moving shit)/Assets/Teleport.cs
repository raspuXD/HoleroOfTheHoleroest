using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform whereToTeleport;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody theRB = other.GetComponent<Rigidbody>();
        theRB.velocity = Vector3.zero;
        other.gameObject.transform.position = whereToTeleport.position;
    }
}
