using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public float speed = 500f;
    Rigidbody2D rb;
    public int damage = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMovement enemy = other.GetComponent<EnemyMovement>();

            if(enemy.isHiding)
            {
                enemy.TakeDamage(1);
            }
            else
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // destroy the bullet
        }
    }
}
