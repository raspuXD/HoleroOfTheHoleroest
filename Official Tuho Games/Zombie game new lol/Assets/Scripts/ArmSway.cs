using UnityEngine;

public class ArmSway : MonoBehaviour
{
    public float swayAmount = 20f;
    public float swaySpeed = 5f;
    public float maxSwayAmount = 20f;
    public float movementSwayAmount = 5f; // Sway amount for movement
    public float jumpSwayMultiplier = 4f;

    private Quaternion initialRotation;
    private PlayerMovement playerMovement;

    void Start()
    {
        initialRotation = transform.localRotation;
        playerMovement = GetComponentInParent<PlayerMovement>(); // Assumes PlayerMovement is on the parent
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Sway based on mouse movement
        Quaternion targetRotationX = Quaternion.AngleAxis(-swayAmount * mouseX, Vector3.up);
        Quaternion targetRotationY = Quaternion.AngleAxis(swayAmount * mouseY, Vector3.right);
        Quaternion targetRotation = initialRotation * targetRotationX * targetRotationY;

        // Sway based on player movement
        if (playerMovement != null)
        {
            Vector2 moveDir = playerMovement.GetCurrentMovementDirection();
            Quaternion moveSwayX = Quaternion.AngleAxis(-movementSwayAmount * moveDir.x, Vector3.up);
            Quaternion moveSwayY = Quaternion.AngleAxis(movementSwayAmount * moveDir.y, Vector3.right);
            targetRotation *= moveSwayX * moveSwayY;

            // Sway based on player jump movement
            if (playerMovement.IsJumping())
            {
                float jumpSway = playerMovement.GetVerticalVelocity() * jumpSwayMultiplier;
                Quaternion jumpSwayRotation = Quaternion.AngleAxis(jumpSway, Vector3.right);
                targetRotation *= jumpSwayRotation;
            }
        }

        // Smoothly interpolate to the target rotation
        targetRotation = Quaternion.Lerp(initialRotation, targetRotation, Mathf.Clamp01(maxSwayAmount));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * swaySpeed);
    }

    public void MoveBack()
    {
        transform.localRotation = initialRotation;
    }
}
