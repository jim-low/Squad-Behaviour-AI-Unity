using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourTree : MonoBehaviour
{
    public Soldier sendiri;
    public Soldier target;

    public Selector root;
    public ActionNode roamingNode;
    public Sequence chaseSequence;
    public Sequence attackSequence;
    public Sequence hideSequence;
    public Sequence reloadSequence;
    public Sequence dieSequence;

    void Start()
    {
        roamingNode = new ActionNode(null /* put in a method to check on whether to roam or not??? */);

        root = new Selector(new List<Node> {
            roamingNode,
                chaseSequence,
                attackSequence,
                hideSequence,
                reloadSequence,
                dieSequence,
        });
    }
}
