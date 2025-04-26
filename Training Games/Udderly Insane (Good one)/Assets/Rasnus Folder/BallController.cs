using System;
using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;  // Direction of the ball's movement
    public float speed = 30f;   // Speed of the ball

    public int life = 3;  // Ball's life count

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 initialDirection)
    {
        direction = initialDirection;
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        life--;

        if(life <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealth component
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Apply damage to the player
                playerHealth.TakeDamage(10);  // Assuming the damage is 10 (can be adjusted)
            }
        }

        // Get the first contact point
        var firstContact = collision.contacts[0];
        
        // Reflect the ball based on the surface normal of the contact
        direction = Vector2.Reflect(direction, firstContact.normal).normalized;
        
        // Apply the new reflected direction to the ball's velocity
        rb.velocity = direction * speed;

        
    }
}
