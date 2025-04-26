using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotateEnemy : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public NavMeshAgent agent;
    public Enemy_Nav nav;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public bool canRotate = true;

    void Update()
    {
        if(canRotate)
        {
            Vector3 toPlayer = nav.playerRef.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, Mathf.Infinity, playerLayer | obstacleLayer);

            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                RotateTowards(toPlayer);
            }
            else
            {
                if (agent.hasPath)
                {
                    Vector3 direction = agent.steeringTarget - transform.position;
                    RotateTowards(direction);
                }
            }
        }
    }

    public void RotateDifferent()
    {
        // Get the current rotation, and adjust it by 180 degrees on the Z-axis.
        float currentZRotation = transform.eulerAngles.z;
        float newZRotation = currentZRotation + 180f;

        // Apply the new rotation
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
    }


    void RotateTowards(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}