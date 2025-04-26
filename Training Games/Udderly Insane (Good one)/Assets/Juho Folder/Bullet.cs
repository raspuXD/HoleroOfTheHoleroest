using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public bool isenemyBullet;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isenemyBullet)
        {
            if(collider.CompareTag("Player"))
            {
                PlayerHealth health = collider.GetComponent<PlayerHealth>();
                if(health != null)
                {
                    health.TakeDamage(Damage);
                }

                Destroy(gameObject);
            }
        }
        else
        {
            if (collider.CompareTag("EnemyCow"))
            {
                Enemy_Health enemyHealth = collider.GetComponent<Enemy_Health>();
                if (enemyHealth != null)
                {
                    // Deal damage to the enemy
                    enemyHealth.TakeDamage(Damage);
                }

                Destroy(gameObject);
            }
        }

        if(collider.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
            
    }
}
