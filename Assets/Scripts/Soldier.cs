using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Transform visor;

    public float sightRange;
    public float sightAngle; // blue
    public float surroundingAwarenessRange; // yellow
    public float keepDistance; // black
    public float followDistance; // green
    public float shootDistance; // red
    [SerializeField] private LayerMask allyLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask coverLayer;

    private bool isLeader;
    private float health;
    private const float MIN_HEALTH = 69;
    private int ammo;
    private const int MAX_AMMO = 30;
    private float damage;
    private List<Transform> coverObjects;
    private List<Transform> enemiesSpotted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemy();
    }

    void DetectEnemy()
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
                }
            }
        }
    }

    // this is just drawing a bunch of things, nothing special lmao
    void OnDrawGizmos()
    {
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
