using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Aim : Node
{
    private Transform transform;
    private float shootDistance;
    private string enemyLayer;

    public Aim(Transform transform, float shootDistance, string enemyLayer)
    {
        this.transform = transform;
        this.shootDistance = shootDistance;
        this.enemyLayer = enemyLayer;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target == null) {
            state = NodeState.FAILURE;
            return state;
        }

        Vector3 lookDirection = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 2f * Time.deltaTime);

        RaycastHit hit;
        bool hitEnemy = Physics.Raycast(transform.position, transform.forward, out hit, shootDistance);
        if (!hitEnemy || hit.collider.tag == "Cover") {
            state = NodeState.FAILURE;
            return state;
        }

        Physics.Raycast(transform.position, transform.forward, out hit, shootDistance, LayerMask.GetMask(enemyLayer));
        if (hit.collider != null) {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
