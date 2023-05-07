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
        Sequence dieSequence = new Sequence(new List<Node> {
            new DepartingOnesLifeToTheOtherWorld(ownData)
        });

        Sequence retreatSequence = new Sequence(new List<Node> {
            new HealthBelowThreshold(ownData)
        });

        Sequence attackSequence = new Sequence(new List<Node> {
            new Detect(ownData.transform, ownData.sightRange, ownData.sightAngle, "Enemy"),
                new Aim(ownData.transform, ownData.shootDistance, "Enemy"),
                new Shoot(ownData, "Enemy"),
        });

        Node root = new Selector(new List<Node> {
            dieSequence,
                retreatSequence,
                attackSequence,
        });
        return root;
    }
}
