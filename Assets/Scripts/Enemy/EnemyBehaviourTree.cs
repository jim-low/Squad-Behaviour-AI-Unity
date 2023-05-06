using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
public class EnemyBehaviourTree : MonoBehaviour
{
    private Soldier sendiri;

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
        sendiri = GetComponent<Soldier>();

        // chase
        checkEnemyWithinSightAngle = new ActionNode();
        chaseEnemy = new ActionNode(null);
        // chase - inverter
        checkChaseDuration = new ActionNode(null);
        stopChasing = new ActionNode(null);

        chaseSequence = new Sequence(new List<Node> {
            checkEnemyWithinSightAngle,
                chaseEnemy,
                chaseInverter,
        });

        // attack
        checkEnemyWithinShootAngle = new ActionNode(sendiri.DetectEnemy);
        shootEnemy = new ActionNode(sendiri.Shoot);

        attackSequence = new Sequence(new List<Node> {
            checkEnemyWithinShootAngle,
                shootEnemy
        });

        // hide
        lowerThan20Percent = new ActionNode(sendiri.IsLowHealth);
        /* hideBehindWall = new ActionNode(); */ // maybe just start healing? cuz idk how to detect if its hiding behind wall
        startHealing = new ActionNode();

        hideSequence = new Sequence(new List<Node> {
            lowerThan20Percent,
            hideBehindWall,
            startHealing,
        });

        // reload

        // death

        rootNode = new Selector(new List<Node> {
                chaseSequence,
                attackSequence,
                hideSequence,
                reloadSequence,
                deathSequence,
        });
    }
}
