using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtActiveCamera : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 direction = cameraTransform.position - transform.position;
            direction.y = 0; // Keep rotation only on the Y-axis (optional)
            transform.rotation = Quaternion.LookRotation(-direction) * Quaternion.Euler(0, 180, 0);
        }
    }
}
