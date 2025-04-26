using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the object collided with has the "EnemyCow" tag
        if (collider.CompareTag("EnemyCow"))
        {
            // Get the EnemyCow's health script (assuming it has one)
            Health enemyHealth = collider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                // Deal damage to the enemy
                enemyHealth.TakeDamage(Damage);
            }

            // Destroy the bullet after it collides (optional)
            Destroy(gameObject);
        }
    }
}
