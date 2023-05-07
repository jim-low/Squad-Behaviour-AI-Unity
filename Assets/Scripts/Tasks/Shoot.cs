using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BehaviorTree;

public class Shoot : Node
{
    private Soldier ownData;
    /* private GameObject bulletLine; */
    /* private bool canShoot; */
    /* private Transform firePoint; */
    /* private float damage; */
    /* private float shootDistance; */
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

        if (!ownData.canShoot) {
            state = NodeState.FAILURE;
            return state;
        }

        if (ownData.ammo <= 0) {
            ownData.Reload();
            state = NodeState.FAILURE;
            return state;
        }

        Vector3 oriScale = ownData.bulletLine.transform.localScale;
        float distance = Vector3.Distance(target.position, ownData.firePoint.position);
        Quaternion bulletRotation = Quaternion.LookRotation(target.position - ownData.firePoint.position);

        ownData.bulletLine.transform.position = ownData.firePoint.transform.position;
        ownData.bulletLine.transform.rotation = bulletRotation;
        ownData.bulletLine.transform.localScale = new Vector3(oriScale.x, oriScale.y, distance);

        // render bullet
        ownData.Recoil();

        // damage enemy
        target.GetComponent<Soldier>().Damage(ownData.damage);

        ownData.ammo -= 1;

        Debug.Log("shoot success");
        state = NodeState.SUCCESS;
        return state;
    }
}
