using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Car : MonoBehaviour
{
    public float maxMotorTorque = 1500f;
    public float maxSteeringAngle = 30f;
    public float maxSpeed;
    public float brakeForce = 4000f; // Increased braking power

    public WheelCollider frontLeftWheel, frontRightWheel;
    public WheelCollider rearLeftWheel, rearRightWheel;

    public Transform frontLeftTransform, frontRightTransform;
    public Transform rearLeftTransform, rearRightTransform;

    public TMP_Text speedText;

    private float acceleration;
    private float steering;
    private Rigidbody rb;
    private float lastAcceleration = 0;

    public bool isPlayerInTheCar = true;
    public bool hasBeenInCarForSecond = false;
    public bool isntDoingAnimation = true;

    public CarEnter leftSide, rightSide;
    public float currentSpeedKmh;

    [SerializeField] KarpasLatka latka;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.drag = 0.1f; // Add small drag to slow the car naturally
        AdjustTireGrip();

        int motorMultiplier = PlayerPrefs.GetInt("MotorUpgrade", 1);
        maxMotorTorque = maxMotorTorque * motorMultiplier;

        int maxSpeedMultiplier = PlayerPrefs.GetInt("SpeedUpgrade", 1);
        maxSpeed += maxSpeedMultiplier * 10;
        brakeForce = brakeForce * (maxSpeedMultiplier / 2f);
    }

    public void StartACountDown()
    {
        StartCoroutine(WaitLol());
    }

    IEnumerator WaitLol()
    {
        yield return new WaitForSeconds(1f);
        hasBeenInCarForSecond = true;
    }

    void Update()
    {
        if(!isPlayerInTheCar)
        {
            ApplyBrakes();
            return;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.Instance.PlayNextMusic();
        }

        float verticalInput = Input.GetAxis("Vertical");

        acceleration = verticalInput * maxMotorTorque;

        float currentSpeed = rb.velocity.magnitude * 3.6f;
        float dynamicSteeringAngle = Mathf.Lerp(maxSteeringAngle, maxSteeringAngle * 0.5f, currentSpeed / 100f);
        steering = Input.GetAxis("Horizontal") * dynamicSteeringAngle;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyBrakes();
        }
        else
        {
            ReleaseBrakes();
        }

        HandleReverseSwitch(verticalInput);

        UpdateWheelTransforms();
        UpdateSpeedUI();


        if(verticalInput == 0 && steering == 0 && rb.velocity.magnitude <= 0.05f && hasBeenInCarForSecond && isntDoingAnimation)
        {
            leftSide.getOut.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E))
            {
                latka.canHit = true;
                latka.theCollider.enabled = false;

                if (leftSide.isOkToLeave)
                {
                    leftSide.DoStuff(false);
                }
                else if (rightSide.isOkToLeave)
                {
                    rightSide.DoStuff(false);
                }
            }
        }
        else
        {
            leftSide.getOut.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if(isntDoingAnimation)
        {
            ApplyAcceleration();
            ApplySteering();
            ApplyAntiRollBar(frontLeftWheel, frontRightWheel);
            ApplyAntiRollBar(rearLeftWheel, rearRightWheel);
        }
    }
    void ApplyAcceleration()
    {
        currentSpeedKmh = rb.velocity.magnitude * 3.6f; // Convert to km/h
        float verticalInput = Input.GetAxis("Vertical");

        // Ensure it does not exceed maxSpeed
        if (currentSpeedKmh >= maxSpeed && verticalInput > 0)
        {
            acceleration = -maxMotorTorque * 0.4f; // Apply a slight counterforce
        }
        else if (currentSpeedKmh >= maxSpeed * 0.5f && verticalInput < 0)
        {
            acceleration = -maxMotorTorque * 0.2f; // Apply less force for reversing
        }
        else
        {
            acceleration = verticalInput * maxMotorTorque;
        }

        // Apply motor torque
        rearLeftWheel.motorTorque = acceleration;
        rearRightWheel.motorTorque = acceleration;
    }



    void ApplySteering()
    {
        frontLeftWheel.steerAngle = steering;
        frontRightWheel.steerAngle = steering;
    }

    void ApplyBrakes()
    {
        rearLeftWheel.brakeTorque = brakeForce;
        rearRightWheel.brakeTorque = brakeForce;
    }

    void ReleaseBrakes()
    {
        rearLeftWheel.brakeTorque = 0;
        rearRightWheel.brakeTorque = 0;
    }

    void HandleReverseSwitch(float verticalInput)
    {
        if (Mathf.Sign(verticalInput) != Mathf.Sign(lastAcceleration) && verticalInput != 0)
        {
            rearLeftWheel.motorTorque = 0;
            rearRightWheel.motorTorque = 0;
            StartCoroutine(ResetAccelerationAfterDelay());
        }
        lastAcceleration = verticalInput;
    }

    IEnumerator ResetAccelerationAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        acceleration = Input.GetAxis("Vertical") * maxMotorTorque;
    }

    void UpdateWheelTransforms()
    {
        UpdateWheelTransform(frontLeftWheel, frontLeftTransform);
        UpdateWheelTransform(frontRightWheel, frontRightTransform);
        UpdateWheelTransform(rearLeftWheel, rearLeftTransform);
        UpdateWheelTransform(rearRightWheel, rearRightTransform);
    }

    void ApplyAntiRollBar(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hit;
        float leftTravel = leftWheel.GetGroundHit(out hit) ? 1 - (hit.point.y - leftWheel.transform.position.y) / leftWheel.suspensionDistance : 1;
        float rightTravel = rightWheel.GetGroundHit(out hit) ? 1 - (hit.point.y - rightWheel.transform.position.y) / rightWheel.suspensionDistance : 1;

        float antiRollForce = (leftTravel - rightTravel) * 5000f;

        if (leftWheel.isGrounded)
            rb.AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);

        if (rightWheel.isGrounded)
            rb.AddForceAtPosition(rightWheel.transform.up * antiRollForce, rightWheel.transform.position);
    }

    void UpdateWheelTransform(WheelCollider collider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    void UpdateSpeedUI()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        float speedKmh = velocity.magnitude * 3.6f;

        if (speedText != null)
        {
            speedText.text = $"{Mathf.RoundToInt(speedKmh)} km/h";
        }
    }

    void AdjustTireGrip()
    {
        AdjustWheelFriction(frontLeftWheel);
        AdjustWheelFriction(frontRightWheel);
        AdjustWheelFriction(rearLeftWheel);
        AdjustWheelFriction(rearRightWheel);
    }

    void AdjustWheelFriction(WheelCollider wheel)
    {
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

        forwardFriction.stiffness = 2.5f;  // More grip in acceleration/braking
        sidewaysFriction.stiffness = 3.0f; // More grip in cornering

        wheel.forwardFriction = forwardFriction;
        wheel.sidewaysFriction = sidewaysFriction;
    }
}
