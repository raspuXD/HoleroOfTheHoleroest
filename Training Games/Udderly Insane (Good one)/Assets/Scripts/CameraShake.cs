using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 initialLocalPosition;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;

    private void Start()
    {
        // Store the initial local position of the camera
        initialLocalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (shakeDuration > 0)
        {
            // Apply random shake offset
            Vector3 shakeOffset = Random.insideUnitCircle * shakeMagnitude;
            transform.localPosition = initialLocalPosition + shakeOffset;

            // Reduce the shake duration over time
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            // Reset to the initial local position
            transform.localPosition = initialLocalPosition;
        }
    }

    // Public method to trigger the shake
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
