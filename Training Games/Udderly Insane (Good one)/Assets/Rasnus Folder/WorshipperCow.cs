using System.Collections;
using UnityEngine;

public class WorshipperCow : MonoBehaviour
{
    public GameObject ballPrefab;  // Ball prefab to be shot
    public float shootForce = 10f;  // Force applied to the ball when shot
    public float shootInterval = 2f;  // Time interval between shots
    public Transform spawnPoint;  // The transform where the ball will spawn from

    // Shooting angle range
    public float minAngle = -45f; // Minimum angle (in degrees)
    public float maxAngle = 45f;  // Maximum angle (in degrees)

    private void Start()
    {
        // If spawn point is not assigned, default to the enemy's position
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }

        // Start shooting balls at intervals
        InvokeRepeating("ShootBall", 1f, shootInterval);
    }

    void ShootBall()
    {
        // If spawn point is assigned, instantiate the ball at the spawn point, otherwise use enemy's position
        Vector2 spawnPosition = spawnPoint != null ? spawnPoint.position : (Vector2)transform.position;
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        // Get the BallController and Rigidbody2D components of the ball
        BallController ballController = ball.GetComponent<BallController>();
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (ballController != null && rb != null)
        {
            // Calculate the shooting angle (in radians)
            float angle = Random.Range(minAngle, maxAngle);
            float angleInRadians = angle * Mathf.Deg2Rad;  // Convert degrees to radians

            // Calculate the direction vector based on the angle
            Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;

            // Call the Shoot method from BallController to set the ball's velocity
            ballController.Shoot(direction);
        }
    }
}
