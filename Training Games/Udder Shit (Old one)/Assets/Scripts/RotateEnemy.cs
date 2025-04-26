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

    void Update()
    {
        Vector3 toPlayer = nav.playerRef.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, Mathf.Infinity, playerLayer | obstacleLayer);

        if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
        {
            Vector3 targetDirection = toPlayer;

            Rigidbody2D playerRigidbody = nav.playerRef.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                Vector3 predictedPosition = (Vector3)playerRigidbody.velocity * 1f + nav.playerRef.transform.position;
                targetDirection = predictedPosition - transform.position;
            }

            RotateTowards(targetDirection);
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

    void RotateTowards(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
