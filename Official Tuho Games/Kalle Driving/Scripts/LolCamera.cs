using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LolCamera : MonoBehaviour
{
 public float maxAngle = 45f; // Maximum angle from the center
    public float sensitivity = 0.2f; // Sensitivity of the camera rotation

    private Vector2 screenCenter;
    private Vector2 mouseDelta;
    private Quaternion originalRotation;

    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mouseDelta = (mousePos - screenCenter) / screenCenter * maxAngle;

        float clampedX = Mathf.Clamp(mouseDelta.y, -maxAngle, maxAngle);
        float clampedY = Mathf.Clamp(mouseDelta.x, -maxAngle, maxAngle);

        transform.localRotation = originalRotation * Quaternion.Euler(-clampedX, clampedY, 0);
    }
}
