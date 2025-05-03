using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFlip : MonoBehaviour
{
    public Transform whatToFlip;
    public float resetForce = 5f;
    private bool canReset = false;
    private bool canTakeDamage = true;
    public bool isInAnimation = false;
    private Rigidbody rb;
    public GameObject theVisualiser;
    CarDurability durability;

    void Start()
    {
        rb = whatToFlip.gameObject.GetComponent<Rigidbody>();
        durability = FindObjectOfType<CarDurability>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && !isInAnimation)
        {
            if(canTakeDamage)
            {
                durability.currentDura--;
                StartCoroutine(ResetDurabilityCooldown());
            }

            Vector3 euler = whatToFlip.eulerAngles;
            if ((euler.x >= 45 && euler.x <= 315) || (euler.z >= 45 && euler.z <= 315))
            {
                canReset = true;
                theVisualiser.SetActive(true);
            }
        }
    }

    IEnumerator ResetDurabilityCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(5f);
        canTakeDamage = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            canReset = false;
            theVisualiser.SetActive(false);
        }
    }

    void Update()
    {
        if (canReset && Input.GetKeyDown(KeyCode.R))
        {
            ResetCar();
        }
    }

    void ResetCar()
    {
        Vector3 newRotation = new Vector3(0, whatToFlip.eulerAngles.y, 0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * resetForce, ForceMode.Impulse);
        whatToFlip.rotation = Quaternion.Euler(newRotation);
        durability.currentDura--;
    }
}
