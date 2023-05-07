using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

[RequireComponent(typeof(NavMeshAgent))]

public class ChaseMovement : Node
{
    private Soldier self;
    private string enemyLayer;
    private NavMeshAgent navMeshAgent;

    public ChaseMovement(Soldier soldier, string enemyLayer)
    {
        self = soldier;
        navMeshAgent = soldier.GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = self.shootDistance * 0.6f;
        navMeshAgent.autoBraking = true;
        navMeshAgent.speed = 12f;
        navMeshAgent.acceleration = navMeshAgent.speed * 0.5f;
        this.enemyLayer = enemyLayer;
    }

    public override NodeState Evaluate()
    {
        if (self.Unalived()) {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)GetData(String.Format(String.Format("{0} target {1}", self.name, enemyLayer)));
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (Vector3.Distance(self.gameObject.transform.position, target.position) < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.velocity = Vector3.zero;
            state = NodeState.FAILURE;
            return state;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.transform.position);

        state = NodeState.SUCCESS;
        return state;
    }
}
