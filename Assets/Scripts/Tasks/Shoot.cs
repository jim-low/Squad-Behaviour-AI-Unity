using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BehaviorTree;

public class Shoot : Node
{
    private Soldier ownData;
    private string enemyLayer;

    public Shoot(Soldier ownData, string enemyLayer)
    {
        this.ownData = ownData;
        this.enemyLayer = enemyLayer;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target == null) {
            state = NodeState.FAILURE;
            return state;
        }

        if (ownData.NeedReload()) {
            ownData.Reload();
            state = NodeState.FAILURE;
            return state;
        }

        ownData.Shoot(target);

        state = NodeState.SUCCESS;
        return state;
    }
}
