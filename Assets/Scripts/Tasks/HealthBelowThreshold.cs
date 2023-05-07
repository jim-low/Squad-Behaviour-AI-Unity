using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            state = NodeState.SUCCESS;
            return state;
        }

        ownData.Hide(false);
        state = NodeState.FAILURE;
        return state;
    }
}
