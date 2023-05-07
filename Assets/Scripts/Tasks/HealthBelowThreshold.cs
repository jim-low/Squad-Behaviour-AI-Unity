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
        if (ownData.IsLowHealth()) {
            ownData.Hide(true);
            state = NodeState.SUCCESS;
            return state;
        }

        ownData.Hide(false);
        state = NodeState.RUNNING;
        return state;
    }
}
