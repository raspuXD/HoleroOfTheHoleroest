using System;
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

    public float jumpHeight = 6f;
    float velocityY;
    bool isGrounded;
    bool isJumping;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateMouse();
        UpdateMove();
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

        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if(isGrounded! && controller.velocity.y < -1f)
        {
            velocityY = -8f;
            isJumping = false;
        }
    }

    public Vector2 GetCurrentMovementDirection()
    {
        return currentDir;
    }

    public bool IsJumping()
    {
        return isJumping; // Return the jump state
    }

    public float GetVerticalVelocity()
    {
        return velocityY; // Return the vertical velocity
    }
}
