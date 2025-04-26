using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageZone : MonoBehaviour
{
    public BoxCollider2D colliderr;
    public bool canDeal = false;
    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null && canDeal)
            {
                health.TakeDamage(10);
            }
        }
    }
}
