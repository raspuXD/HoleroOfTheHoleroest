using UnityEngine;

public class SmoothRotationWithBottomDirection : MonoBehaviour
{
    public float rotationSpeed = 5f; // Speed of rotation towards movement direction
    public float biasAmount = 0.2f; // The amount of bias to apply to direction (optional)
    private Vector2 currentDirection; // Current direction of movement

    private void Update()
    {
        // Retrieve the current movement direction from the VirusBreaker script
        VirusBreaker virusBreaker = GetComponent<VirusBreaker>();
        if (virusBreaker != null)
        {
            currentDirection = virusBreaker.direction;
        }

        // Apply bias if needed (optional)
        Vector2 biasedDirection = ApplyRotationBias(currentDirection);

        // Call the method to rotate smoothly with the bottom facing the movement direction
        SmoothRotateToDirection(biasedDirection);
    }

    private Vector2 ApplyRotationBias(Vector2 direction)
    {
        // Apply a bias to the direction vector (if you want to modify movement bias)
        float xBias = Mathf.Sign(direction.x) * biasAmount;
        float yBias = Mathf.Sign(direction.y) * biasAmount;

        return direction + new Vector2(xBias, yBias);
    }

    private void SmoothRotateToDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Calculate the angle to rotate towards, in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Adjust the angle to make the bottom of the sprite face the direction
            // To make the bottom face the direction, you need to rotate by 90 degrees (π/2 radians).
            angle += 90f;

            // Smoothly rotate towards the desired angle using Quaternion.Lerp
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
