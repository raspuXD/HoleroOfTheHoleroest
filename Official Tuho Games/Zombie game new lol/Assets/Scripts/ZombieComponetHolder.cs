using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieComponetHolder : MonoBehaviour
{
    [SerializeField] NavMeshAgent agentto;
    [SerializeField] ZombieFollow ZombieFollow;

    public void ActivateAllComponents(bool whatToDo)
    {
        agentto.enabled = whatToDo;
        ZombieFollow.enabled = whatToDo;
    }
}
