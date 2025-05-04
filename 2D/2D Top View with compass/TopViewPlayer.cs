using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopViewPlayer : MonoBehaviour
{
    public float speed = 5f;
    public float theRotationSpeed = 400f;

    private Rigidbody2D rb;

    [Header("Rotate towards mouse")]
    public bool doesRotateTowards = true;
    public Transform theModel;
    private float mx;
    private float my;
    private Vector2 mousePos;

    [Header("Sprinting")]
    public bool canSprint = true;
    public bool requiresHoldToSprint = true;
    public float runSpeed = 8f;
    public float howLongCanSprint = 5f;
    private float sprintTime;
    public Image staminaFill;
    public TrailRenderer[] trailsWhenRunning;

    private bool isSprinting;
    private bool hasSprinted = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprintTime = howLongCanSprint;

        //uncomment this when toggle in opitons
        /*
        int whatToggleSays = PlayerPrefs.GetInt("HowDoesSprint", 0);
        if(whatToggleSays == 0)
        {
            requiresHoldToSprint = true;
        }
        else
        {
            requiresHoldToSprint = false;
        }
        */
    }

    private void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (doesRotateTowards)
        {
            float angleToMouse = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angleToMouse);
            theModel.rotation = Quaternion.RotateTowards(theModel.rotation, targetRotation, theRotationSpeed * Time.deltaTime);
        }

        if (canSprint)
        {
            if (requiresHoldToSprint)
            {
                if (Input.GetKey(KeyCode.LeftShift) && !isSprinting && sprintTime >= howLongCanSprint * 0.15f && (mx != 0 || my != 0))
                {
                    isSprinting = true;
                }

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    isSprinting = false;
                }

                if (isSprinting && sprintTime <= 0f)
                {
                    isSprinting = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && sprintTime >= howLongCanSprint * 0.15f && !hasSprinted && (mx != 0 || my != 0))
                {
                    isSprinting = !isSprinting;
                    hasSprinted = true;
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    hasSprinted = false;
                }

                if (isSprinting && sprintTime <= 0f)
                {
                    isSprinting = false;
                }
            }

            if (!isSprinting && sprintTime < howLongCanSprint)
            {
                sprintTime += Time.deltaTime;
            }

            if (isSprinting)
            {
                sprintTime -= Time.deltaTime;
            }

            if (staminaFill != null)
            {
                staminaFill.fillAmount = sprintTime / howLongCanSprint;
            }
        }

    }

    private void FixedUpdate()
    {
        Vector2 moveDirection = new Vector2(mx, my).normalized;

        if (isSprinting)
        {
            rb.velocity = moveDirection * runSpeed;
            foreach (TrailRenderer trail in trailsWhenRunning)
            {
                trail.emitting = true;
            }
        }
        else
        {
            rb.velocity = moveDirection * speed;
            foreach (TrailRenderer trail in trailsWhenRunning)
            {
                trail.emitting = false;
            }
        }

        if (!doesRotateTowards && moveDirection.sqrMagnitude > 0)
        {
            float angleToMove = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angleToMove);
            theModel.rotation = Quaternion.RotateTowards(theModel.rotation, targetRotation, theRotationSpeed * Time.fixedDeltaTime);
        }
    }
}
