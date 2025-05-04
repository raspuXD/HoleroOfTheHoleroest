using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform cameraHolder;

    [Header("View Bobbing")]
    public float baseBobFrequency = 15f; // Base frequency of bobbing
    public float walkBobAmplitude = 0.1f; // Bob amplitude while walking
    public float sprintBobAmplitude = 0.2f; // Increased amplitude while sprinting
    public float bobSmoothing = 10f;  // How fast the camera transitions to the target position
    public float walkBobFrequencyMultiplier = 0.1f; // Multiplier for walk frequency
    public float sprintBobFrequencyMultiplier = 0.2f; // Multiplier for sprint frequency

    private float xRotation;
    private float yRotation;
    private float bobTimer;

    public PlayerMovementAdvanced playerMovement; // Reference to PlayerMovement script
    private Vector3 originalPosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Save the original position of the cameraHolder
        originalPosition = cameraHolder.localPosition;

        // Ensure the playerMovement reference is set
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovementAdvanced script not assigned to PlayerCam.");
        }
    }

    private void Update()
    {
        HandleMouseLook();
        ApplyViewBobbing();
    }

    private void HandleMouseLook()
    {
        // Mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // Update camera rotation
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 89f);

        // Apply rotation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void ApplyViewBobbing()
    {
        if (playerMovement == null || cameraHolder == null)
        {
            Debug.LogWarning("PlayerMovement or cameraHolder is not set.");
            return;
        }

        // Calculate player's velocity
        Vector3 velocity = playerMovement.rb.velocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        float speed = horizontalVelocity.magnitude;

        if (speed > 0.1f && playerMovement.grounded)
        {
            // Determine bob amplitude and frequency based on player state
            float bobAmplitude = walkBobAmplitude;
            float bobFrequencyMultiplier = walkBobFrequencyMultiplier;

            if (playerMovement.state == PlayerMovementAdvanced.MovementState.sprinting)
            {
                bobAmplitude = sprintBobAmplitude;
                bobFrequencyMultiplier = sprintBobFrequencyMultiplier;
            }

            // Adjust bob frequency based on player speed and state
            float dynamicBobFrequency = baseBobFrequency + (speed * bobFrequencyMultiplier);

            // Update bobbing timer
            bobTimer += Time.deltaTime * dynamicBobFrequency;

            // Calculate bobbing offset
            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

            // Smoothly transition to the bobbing position
            Vector3 targetPosition = new Vector3(0, originalPosition.y + bobOffset, 0);
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, targetPosition, Time.deltaTime * bobSmoothing);
        }
        else
        {
            // Smoothly return to the original position when not moving
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, originalPosition, Time.deltaTime * bobSmoothing);
        }
    }
}
