using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Nav : MonoBehaviour
{
    public Transform playerRef;
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;
    public float stopDistance = 1f;
    NavMeshAgent agent;
    public EnemyShoot shoot;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = movementSpeed;
        if(shoot != null)
        {
            shoot.target = playerRef;
        }
    }

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (Vector3.Distance(transform.position, playerRef.position) <= stopDistance)
            return;

        agent.SetDestination(playerRef.position);
    }
}
