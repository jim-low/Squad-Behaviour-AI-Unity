using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using BehaviorTree;

public class HealthBelowThreshold : Node
{
    private Soldier ownData;

    public HealthBelowThreshold(Soldier ownData)
    {
        this.ownData = ownData;
    }

    public override NodeState Evaluate()
    {
        if (ownData.Unalived()) {
            state = NodeState.FAILURE;
            return state;
        }

        if (ownData.IsLowHealth()) {
            ownData.Hide(true);
            ownData.Heal();
            ownData.GetComponent<NavMeshAgent>().speed = 15f;
            state = NodeState.SUCCESS;
            return state;
        }

        ownData.Hide(false);
        state = NodeState.FAILURE;
        return state;
    }
}
