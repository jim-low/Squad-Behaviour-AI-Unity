using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourTree : MonoBehaviour
{
    private Soldier soldierData;
    private Soldier ownData;

    //define nodes
    public Selector rootNode;
    public ActionNode startRoaming;

    //chase sequence Branch --------------------------
    public Sequence chaseSequence;
    public ActionNode checkEnemyWithinSightAngle;
    public ActionNode chaseEnemy;
    public Inverter chaseInverter;
    public ActionNode checkChaseDuration;
    public ActionNode stopChasing;
    //------------------------------------------------

    //Attack sequence Branch --------------------------
    public Sequence attackSequence;
    public ActionNode checkEnemyWithinShootAngle;
    public ActionNode shootEnemy;
    //------------------------------------------------

    //Hide sequence Branch --------------------------
    public Sequence hideSequence;
    public ActionNode lowerThan20Percent;
    public ActionNode hideBehindWall;
    public ActionNode startHealing;
    //------------------------------------------------

    //reload sequence Branch --------------------------
    public Sequence reloadSequence;
    public ActionNode checkAmmo;
    public ActionNode reload;
    //-------------------------------------------------

    //die sequence Branch --------------------------
    public Sequence deathSequence;
    public ActionNode playDeathAnimation;
    //-------------------------------------------------

    //define tree functions
    public delegate void TreeExecuted();
    public event TreeExecuted onTreeExecuted;

    public delegate void NodePassed(string trigger);

    void Start()
    {
        /* roamingNode = new ActionNode(null /1* put in a method to check on whether to roam or not??? *1/); */

        rootNode = new Selector(new List<Node> {
            /* roamingNode, */
                chaseSequence,
                attackSequence,
                hideSequence,
                reloadSequence,
                deathSequence,
        });
    }
}
