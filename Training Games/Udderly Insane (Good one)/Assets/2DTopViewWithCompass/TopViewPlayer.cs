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

    public Image[] imagesToFadeWhenFull;
    public float howLongFullUntilFade;

    private bool isSprinting;
    private bool hasSprinted = false;
    private float fullStaminaTime;
    private bool isFading;

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
            Quaternion targetRotation = Quaternion.Euler(0, 0, angleToMouse + 90f);
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
                if (!Input.GetKey(KeyCode.LeftShift) || sprintTime <= 0f)
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

        HandleStaminaFading();
    }

    private void HandleStaminaFading()
    {
        if (sprintTime >= howLongCanSprint)
        {
            if (!isFading)
            {
                fullStaminaTime += Time.deltaTime;
                if (fullStaminaTime >= howLongFullUntilFade)
                {
                    StartCoroutine(FadeImages(0f));
                    isFading = true;
                }
            }
        }
        else
        {
            fullStaminaTime = 0f;
            if (isFading)
            {
                StartCoroutine(FadeImages(1f));
                isFading = false;
            }
        }
    }

    private IEnumerator FadeImages(float targetAlpha)
    {
        float duration = 0.35f;
        float elapsed = 0f;
        Color[] initialColors = new Color[imagesToFadeWhenFull.Length];

        for (int i = 0; i < imagesToFadeWhenFull.Length; i++)
        {
            initialColors[i] = imagesToFadeWhenFull[i].color;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(initialColors[0].a, targetAlpha, elapsed / duration);
            for (int i = 0; i < imagesToFadeWhenFull.Length; i++)
            {
                Color newColor = imagesToFadeWhenFull[i].color;
                newColor.a = alpha;
                imagesToFadeWhenFull[i].color = newColor;
            }
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveDirection = new Vector2(mx, my).normalized;

        rb.velocity = moveDirection * (isSprinting ? runSpeed : speed);
        foreach (TrailRenderer trail in trailsWhenRunning)
        {
            trail.emitting = isSprinting;
        }

        if (!doesRotateTowards && moveDirection.sqrMagnitude > 0)
        {
            float angleToMove = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angleToMove);
            theModel.rotation = Quaternion.RotateTowards(theModel.rotation, targetRotation, theRotationSpeed * Time.fixedDeltaTime);
        }
    }
}
