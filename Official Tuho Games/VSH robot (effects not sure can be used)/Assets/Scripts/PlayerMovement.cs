using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSmoothTime = .03f;
    [SerializeField] float mouseSensivity = 3.5f;
    [SerializeField] float speed = 5f;
    [SerializeField] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [SerializeField] float headBobFrequency = 1.5f; // Head bob frequency
    [SerializeField] float headBobAmplitude = 0.05f; // Head bob amplitude

    float velocityY;
    bool isGrounded;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;

    [SerializeField] AudioClip walkingSound1, walkingSound2;
    public AudioSource audioSource;

    private bool isPlayingWalkingSound = false; // To control sound playback
    private bool alternateFoot = false; // To alternate between sounds
    private Vector3 initialCameraPosition; // Initial position of the camera
    private float headBobTimer; // Timer for head bobbing

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialCameraPosition = playerCamera.localPosition; // Store the initial camera position
    }

    private void Update()
    {
        UpdateMouse();
        UpdateMove();
        UpdateHeadBobbing();
    }

    private void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraCap -= currentMouseDelta.y * mouseSensivity;
        cameraCap = Mathf.Clamp(cameraCap, -90f, 90f);
        playerCamera.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensivity);
    }

    private void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY += gravity * 2f * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        // Play walking sound if grounded and moving
        if (currentDir.magnitude > 0.1f)
        {
            if (!isPlayingWalkingSound)
            {
                PlayWalkingSound();
            }
        }
        else
        {
            // Stop the walking sound if not moving
            audioSource.Stop();
        }
    }

    private void UpdateHeadBobbing()
    {
        if (currentDir.magnitude > 0.1f && isGrounded)
        {
            // Increment the bobbing timer based on the movement
            headBobTimer += Time.deltaTime * headBobFrequency;

            // Calculate vertical offset using sine wave
            float bobOffset = Mathf.Sin(headBobTimer) * headBobAmplitude;

            // Apply the vertical offset to the camera's position
            playerCamera.localPosition = initialCameraPosition + new Vector3(0, bobOffset, 0);
        }
        else
        {
            // Reset head bobbing when the player is stationary
            headBobTimer = 0;
            playerCamera.localPosition = initialCameraPosition;
        }
    }


    private void PlayWalkingSound()
    {
        // Toggle between the two walking sounds
        audioSource.pitch = Random.Range(0.5f, 1.5f);
        audioSource.clip = alternateFoot ? walkingSound1 : walkingSound2;
        audioSource.Play();
        isPlayingWalkingSound = true;
        alternateFoot = !alternateFoot;

        // Schedule to play the next sound after the current one ends
        StartCoroutine(ResetWalkingSound());
    }

    private IEnumerator ResetWalkingSound()
    {
        yield return new WaitForSeconds(.4f);
        isPlayingWalkingSound = false;
    }

    public Vector2 GetCurrentMovementDirection()
    {
        return currentDir;
    }

    public float GetVerticalVelocity()
    {
        return velocityY;
    }
}
