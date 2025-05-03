using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSmoothTime = .03f;
    [SerializeField] float mouseSensivity = 3.5f;
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [SerializeField] float headBobFrequency = 1.5f;
    [SerializeField] float headBobAmplitude = 0.05f;
    [SerializeField] float sprintHeadBobMultiplier = 2f; // Multiplier for sprinting head bobbing

    float velocityY;
    bool isGrounded;
    bool isSprinting;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;

    public GameObject pauseCanvas;

    [SerializeField] AudioClip walkingSound1, walkingSound2;
    public AudioSource audioSource;

    private bool isPlayingWalkingSound = false;
    private bool alternateFoot = false;
    private Vector3 initialCameraPosition;
    private float headBobTimer;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialCameraPosition = playerCamera.localPosition;
    }

    private void Update()
    {
        if (pauseCanvas.activeSelf) return;

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
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY += gravity * 2f * Time.deltaTime;

        float currentSpeed = isSprinting ? sprintSpeed : speed;
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (currentDir.magnitude > 0.1f && isGrounded)
        {
            if (!isPlayingWalkingSound)
            {
                PlayWalkingSound();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void UpdateHeadBobbing()
    {
        if (currentDir.magnitude > 0.1f && isGrounded)
        {
            float bobFrequency = isSprinting ? headBobFrequency * sprintHeadBobMultiplier : headBobFrequency;
            float bobAmplitude = isSprinting ? headBobAmplitude * sprintHeadBobMultiplier : headBobAmplitude;

            headBobTimer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(headBobTimer) * bobAmplitude;
            playerCamera.localPosition = initialCameraPosition + new Vector3(0, bobOffset, 0);
        }
        else
        {
            headBobTimer = 0;
            playerCamera.localPosition = initialCameraPosition;
        }
    }

    private void PlayWalkingSound()
    {
        audioSource.pitch = Random.Range(0.5f, 1.5f);
        audioSource.clip = alternateFoot ? walkingSound1 : walkingSound2;
        audioSource.Play();
        isPlayingWalkingSound = true;
        alternateFoot = !alternateFoot;

        StartCoroutine(ResetWalkingSound());
    }

    private IEnumerator ResetWalkingSound()
    {
        float waitTime = isSprinting ? 0.3f : 0.4f; // Halve time when sprinting
        yield return new WaitForSeconds(waitTime);
        isPlayingWalkingSound = false;
    }

    public Vector2 GetCurrentMovementDirection() => currentDir;
    public float GetVerticalVelocity() => velocityY;
}
