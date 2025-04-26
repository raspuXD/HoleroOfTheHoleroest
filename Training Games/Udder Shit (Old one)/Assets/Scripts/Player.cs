using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private float mx;
    private float my;

    private Vector2 mousePos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get movement input
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");

        // Get mouse position in world space and set z to 0 (2D)
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the angle to the mouse position
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        // Rotate the player towards the mouse position
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate()
    {
        // Move the player based on input
        rb.velocity = new Vector2(mx, my).normalized * speed;
    }
}
