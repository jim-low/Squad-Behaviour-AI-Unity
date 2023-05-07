using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class SoldierBehaviorTree : Tree
{
    private Soldier ownData;
    private string enemyLayer;

    void Awake()
    {
        this.ownData = GetComponent<Soldier>();
        this.enemyLayer = ownData.GetEnemyLayer();
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
            new Detect(ownData, ownData.transform, ownData.sightRange, ownData.sightAngle, enemyLayer),
                new Aim(ownData.transform, ownData.shootDistance, enemyLayer),
                new Shoot(ownData, enemyLayer),
        });

        Sequence chaseSequence = new Sequence(new List<Node> {
            new Detect(ownData, ownData.transform, ownData.sightRange, ownData.sightAngle, enemyLayer),
            new ChaseMovement(ownData, enemyLayer),
        });

        Node root = new Selector(new List<Node> {
            dieSequence,
                retreatSequence,
                chaseSequence,
                attackSequence,
        });
        return root;
    }
}
