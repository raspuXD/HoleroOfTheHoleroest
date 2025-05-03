using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFollow : MonoBehaviour
{
    Transform player;
    //public Animator animator;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.transform;
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            player = playerObject.transform;
        }

        if(agent.isOnOffMeshLink)
        {
            var meshLink = agent.currentOffMeshLinkData;

            if(meshLink.offMeshLink.area == NavMesh.GetAreaFromName("Window"))
            {
                //play window climping animation
                agent.speed = .5f;
                //animator.Play("ZombieClimp");
            }
        }
        else
        {
            agent.speed = 3.5f;
            //animator.Play("ZombieWalk");
        }

        agent.destination = player.position;
    }
}
