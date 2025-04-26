using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Nav : MonoBehaviour
{
    public Transform playerRef;
    public Transform cowRef;
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;
    public float stopDistance = 1f;
    public float cowSwitchDistance = 5f;
    private bool isCowTarget = false;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = movementSpeed;
    }

    private void Update()
    {
        if (!isCowTarget)
        {
            CheckCowProximity();
        }

        if(cowRef == null)
        {
            isCowTarget = false;
        }

        FollowTarget();
    }

    private void CheckCowProximity()
    {
        if(cowRef != null)
        {
            if (Vector3.Distance(transform.position, cowRef.position) <= cowSwitchDistance)
            {
                isCowTarget = true;
            }
        }
    }

    private void FollowTarget()
    {
        Transform target = isCowTarget ? cowRef : playerRef;

        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
            return;

        agent.SetDestination(target.position);
    }

    private void OnDrawGizmos()
    {
        // Draw the stop distance around the enemy
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        // Draw the cow switch distance around the cow
        if (cowRef != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(cowRef.position, cowSwitchDistance);
        }

        // Draw a line to the current target
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Transform target = isCowTarget ? cowRef : playerRef;
            if (target != null)
            {
                Gizmos.DrawLine(transform.position, target.position);
            }
        }
    }
}
