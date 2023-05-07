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
            Debug.Log(ownData.gameObject.name + " is on low health!!! D:");
            ownData.Hide(true);
            ownData.Heal();
            state = NodeState.SUCCESS;
            return state;
        }

        Debug.Log(ownData.gameObject.name + " is good fam");
        ownData.Hide(false);
        state = NodeState.FAILURE;
        return state;
    }
}
