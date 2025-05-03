using UnityEngine;

public class ArmSway : MonoBehaviour
{
    public float swayAmount = 20f;
    public float swaySpeed = 5f;
    public float maxSwayAmount = 20f;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Quaternion targetRotationX = Quaternion.AngleAxis(-swayAmount * mouseX, Vector3.up);
        Quaternion targetRotationY = Quaternion.AngleAxis(swayAmount * mouseY, Vector3.right);
        Quaternion targetRotation = initialRotation * targetRotationX * targetRotationY;
        targetRotation = Quaternion.Lerp(initialRotation, targetRotation, Mathf.Clamp01(maxSwayAmount));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * swaySpeed);
    }

    public void MoveBack()
    {
        transform.localRotation = initialRotation;
    }
}
