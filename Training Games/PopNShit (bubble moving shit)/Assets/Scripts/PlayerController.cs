using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float crouchSpeed = 2.5f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    private float originalHeight;

    [Header("Physics")]
    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance = 0.2f;

    [Header("Random Movement Settings")]
    public float randomMoveInterval = 5f;
    public float randomMoveDuration = 1f;
    public float randomMoveSpeed = 4f;
    public float smoothTransitionSpeed = 5f;
    public float randomDirectionVariance = 0.5f;

    public CameraController cameraController;
    public PickUp pickUp;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private bool isGrounded;
    private bool isCrouching;
    private bool isRandomMoving;
    private Vector3 randomDirection;
    private float randomMoveTimer;
    private float randomMoveEndTime;

    [Header("If collided")]
    public float minTimeUntilAgain, maxTimeUntilAgain;
    private float nextTriggerTime;
    int shouldHaveHappened = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalHeight = capsuleCollider.height;
        randomMoveTimer = randomMoveInterval;
        nextTriggerTime = Time.time;
    }

    void Update()
    {
        if (!isRandomMoving)
        {
            HandleManualMovement();
        }
        else
        {
            HandleRandomMovement();
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isRandomMoving)
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }

        if (rb.velocity.magnitude > 0.001f)
        {
            randomMoveTimer -= Time.deltaTime;
            if (randomMoveTimer <= 0f && !isRandomMoving)
            {
                TriggerRandomMovement();
            }
        }
        else
        {
            randomMoveTimer = randomMoveInterval;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerKompastus") && Time.time >= nextTriggerTime)
        {
            TriggerRandomMovement();
            nextTriggerTime = Time.time + Random.Range(minTimeUntilAgain, maxTimeUntilAgain);
        }
    }

    void HandleManualMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

        float speed = isCrouching ? crouchSpeed : walkSpeed;

        if (pickUp.canDrop && pickUp.heldObj != null)
        {
            InteractableObject theObject;
            theObject = pickUp.heldObj.GetComponent<InteractableObject>();
            speed = theObject.movemeventSpeed;
        }

        Vector3 targetVelocity = move * speed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;
    }

    void TriggerRandomMovement()
    {
        if (isGrounded)
        {
            isRandomMoving = true;
            randomMoveEndTime = Time.time + randomMoveDuration;

            Vector3 currentDirection = rb.velocity.normalized;
            if (currentDirection == Vector3.zero)
            {
                currentDirection = transform.forward;
            }

            float randomX = Random.Range(-randomDirectionVariance, randomDirectionVariance);
            float randomZ = Random.Range(randomDirectionVariance, -randomDirectionVariance);
            randomDirection = new Vector3(currentDirection.x + randomX, 0f, currentDirection.z + randomZ).normalized;

            cameraController.TriggerRandomRotation(true, randomDirection);

            AudioManager.Instance.PlaySFX("Kompastus1");
            AudioManager.Instance.PlaySFX("Kompastus2");

            if (pickUp.canDrop && pickUp.heldObj != null)
            {
                int chanceToHappen = Random.Range(0, 100);

                if (chanceToHappen > 60 || shouldHaveHappened >= 2)
                {
                    int chanceToThrow = Random.Range(0, 100);

                    if (chanceToThrow > 80)
                    {
                        pickUp.ThrowObject();
                    }
                    else
                    {
                        pickUp.DropObject();
                    }

                    shouldHaveHappened = 0;
                }
                else
                {
                    shouldHaveHappened++;
                }
            }
        }
    }

    void HandleRandomMovement()
    {
            Vector3 targetVelocity = randomDirection * randomMoveSpeed;
            targetVelocity.y = rb.velocity.y;
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * smoothTransitionSpeed);

            if (Time.time >= randomMoveEndTime)
            {
                isRandomMoving = false;
                randomMoveTimer = randomMoveInterval;

                cameraController.TriggerRandomRotation(false, Vector3.zero);
            }
    }

    void Crouch()
    {
        isCrouching = true;
        capsuleCollider.height = crouchHeight;
    }

    void StandUp()
    {
        isCrouching = false;
        capsuleCollider.height = originalHeight;
    }
}
