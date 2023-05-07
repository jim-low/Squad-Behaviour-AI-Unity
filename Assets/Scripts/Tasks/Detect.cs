using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Detect : Node
{
    private Soldier ownData;
    private Transform transform;
    private string enemyLayer;
    private float sightRange;
    private float sightAngle;

    public Detect(Soldier ownData, Transform transform, float sightRange, float sightAngle, string enemyLayer)
    {
        this.ownData = ownData;
        this.transform = transform;
        this.sightRange = sightRange;
        this.sightAngle = sightAngle;
        this.enemyLayer = enemyLayer;
    }

    public override NodeState Evaluate()
    {
        if (ownData.Unalived()) {
            state = NodeState.FAILURE;
            return state;
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask(enemyLayer));

        float prevDistance = 0;
        Transform closestEnemy = null;
        foreach (Collider enemy in enemies) {
            if (enemy.GetComponent<Soldier>().Unalived()) {
                continue;
            }

            if (Vector3.Angle((enemy.transform.position - transform.position), transform.forward) > sightAngle) {
                continue;
            }

            if (closestEnemy == null) {
                closestEnemy = enemy.transform;
                prevDistance = Vector3.Distance(enemy.transform.position, transform.position);
            }
            else {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < prevDistance) {
                    closestEnemy = enemy.transform;
                    prevDistance = distance;
                }
            }
        }

        if (closestEnemy != null) {
            SetData(String.Format("{0} target {1}", transform.name, enemyLayer), closestEnemy);
            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
