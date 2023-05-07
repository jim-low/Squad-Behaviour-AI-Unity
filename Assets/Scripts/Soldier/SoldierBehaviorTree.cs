using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class SoldierBehaviorTree : Tree
{
    private Soldier ownData;

    void Awake()
    {
        ownData = GetComponent<Soldier>();
    }

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node> {
            new Sequence(new List<Node> {
                new Detect(ownData.transform, ownData.sightRange, ownData.sightAngle, "Enemy"),
                new Aim(ownData.transform, ownData.shootDistance, "Enemy"),
                new Shoot(ownData, "Enemy"),
            })
        });
        return root;
    }
}
