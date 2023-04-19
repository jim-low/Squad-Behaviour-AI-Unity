using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Transform visor;

    [SerializeField] private bool isLeader;

    public float sightRange;
    public float sightAngle; // blue
    public float surroundingAwarenessRange; // yellow, keeps awareness of surrounding covers
    public float keepDistance; // black
    public float followDistance; // green
    public float shootDistance; // red
    [SerializeField] private LayerMask allyLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask coverLayer;

    private float health;
    private const float MIN_HEALTH = 69;
    private int ammo;
    private const int MAX_AMMO = 30;
    private float damage;
    [SerializeField] private List<Transform> coverObjects;
    [SerializeField] private List<Transform> enemiesSpotted;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        ammo = 30;
        damage = 5;
    }

    // Update is called once per frame
    void Update()
    {
        /* DetectEnemies(); */
    }

    private void DetectEnemies()
    {
        if (Physics.CheckSphere(transform.position, sightRange, enemyLayer))
        {
            Collider[] hitTargets = Physics.OverlapSphere(transform.position, sightRange, enemyLayer);
            foreach (var target in hitTargets)
            {
                Enemy enemy = target.gameObject.GetComponent<Enemy>();
                Vector3 enemyPos = enemy.transform.position - transform.position;
                float angle = Mathf.Abs(Vector3.Angle(enemyPos, transform.forward));

                if (angle <= sightAngle && !enemy.isSpotted())
                {
                    enemy.SetDetection(true);
                    enemiesSpotted.Add(enemy.transform);
                }
            }
        }
    }

    // this is just drawing a bunch of things, nothing special lmao
    void OnDrawGizmosSelected()
    {
        if (isLeader)
        {
            Gizmos.DrawIcon(transform.position + (Vector3.up * 2f), "leader.png", true);
        }

        // field of view
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(visor.position, Quaternion.AngleAxis(sightAngle, transform.up) * visor.forward * sightRange);
        Gizmos.DrawRay(visor.position, Quaternion.AngleAxis(-sightAngle, transform.up) * visor.forward * sightRange);

        // keep distance from other soldiers
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, keepDistance);

        // surrounding awareness range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, surroundingAwarenessRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(visor.position, visor.transform.forward * shootDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followDistance);
    }
}
