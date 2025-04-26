using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTravel : MonoBehaviour
{
    public Transform[] targets;  // Array of target transforms
    public float speed = 5f;     // Speed of movement
    public float timeLimit = 5f; // Time limit in seconds
    public float rotationSpeed = 500f; // Speed of rotation

    private Transform target;
    private float timer;

    void Start()
    {
        // Select the first random target
        SetNewTarget();
    }

    void Update()
    {
        // Move towards the selected target
        if (target != null)
        {
            // Move object towards the target
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Rotate the object towards the target
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 180f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the object has reached the target or if time limit has passed
            if (Vector2.Distance(transform.position, target.position) < 0.1f || timer >= timeLimit)
            {
                SetNewTarget();
            }
        }
    }

    void SetNewTarget()
    {
        // Reset the timer and pick a new random target
        target = targets[Random.Range(0, targets.Length)];
        timer = 0f;
    }
}
