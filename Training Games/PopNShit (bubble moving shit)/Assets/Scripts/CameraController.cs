using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    public float randomMoveRotationAngle = 15f; // Maximum tilt angle for X and Z during random movement
    public float rotationResetSpeed = 5f;      // Speed at which the camera resets to 0 rotation

    private float xRotation = 0f;
    private float currentZRotation = 0f;      // Current Z rotation
    private float targetZRotation = 0f;       // Target Z rotation
    private float currentXRotation = 0f;      // Current X rotation (for random movement tilt)
    private float targetXRotation = 0f;       // Target X rotation during random movement

    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSens", 500f);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleRotationReset();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Combine rotations: X for mouse look, current X and Z for random movement
        transform.localRotation = Quaternion.Euler(xRotation + currentXRotation, 0f, currentZRotation);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void TriggerRandomRotation(bool isRandomMoving, Vector3 randomDirection)
    {
        if (isRandomMoving)
        {
            // Adjust Z rotation based on random X direction
            targetZRotation = randomDirection.x > 0 ? randomMoveRotationAngle : -randomMoveRotationAngle;

            // Adjust X rotation based on random Z direction
            targetXRotation = randomDirection.z > 0 ? randomMoveRotationAngle : -randomMoveRotationAngle;
        }
        else
        {
            targetZRotation = 0f; // Reset Z rotation
            targetXRotation = 0f; // Reset X rotation
        }
    }

    void HandleRotationReset()
    {
        // Smoothly interpolate the current rotations back to their target rotations
        currentZRotation = Mathf.Lerp(currentZRotation, targetZRotation, Time.deltaTime * rotationResetSpeed);
        currentXRotation = Mathf.Lerp(currentXRotation, targetXRotation, Time.deltaTime * rotationResetSpeed);
    }
}
