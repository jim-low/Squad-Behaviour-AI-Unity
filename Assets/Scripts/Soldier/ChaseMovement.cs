using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

[RequireComponent(typeof(NavMeshAgent))]

public class ChaseMovement : Node
{
    private Soldier self;
    private NavMeshAgent navMeshAgent;

    public ChaseMovement(Soldier soldier)
    {
        self = soldier;
        navMeshAgent = soldier.GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = self.shootDistance * 0.6f;
        navMeshAgent.autoBraking = true;
        navMeshAgent.speed = 7f;
        navMeshAgent.acceleration = navMeshAgent.speed * 0.25f;

    }

    public override NodeState Evaluate()
    {
        Transform targetToChase = (Transform)GetData("target");
        if (targetToChase == null || navMeshAgent == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (Vector3.Distance(self.gameObject.transform.position, targetToChase.position) < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.velocity = Vector3.zero;
            state = NodeState.SUCCESS;
            return state;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(targetToChase.transform.position);
        

        state = NodeState.RUNNING;
        return state;
    }
}
