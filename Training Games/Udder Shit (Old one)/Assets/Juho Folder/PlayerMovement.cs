using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform childObject; // Assign the child object in the Inspector

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle movement input
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // Rotate child object towards mouse
        RotateChildTowardsMouse();
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.velocity = movement * moveSpeed;
    }

    void RotateChildTowardsMouse()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate direction to mouse
        Vector2 direction = (mousePosition - childObject.position).normalized;

        // Calculate angle and rotate child object
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        childObject.rotation = Quaternion.Euler(0, 0, angle);
    }
}
