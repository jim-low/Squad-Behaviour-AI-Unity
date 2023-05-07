using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DepartingOnesLifeToTheOtherWorld : Node
{
    private Soldier ownData;

    public DepartingOnesLifeToTheOtherWorld(Soldier ownData) 
    {
        this.ownData = ownData;
    }

    public override NodeState Evaluate()
    {
        if (ownData.Unalived()) {
            ownData.DieLmao();
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
