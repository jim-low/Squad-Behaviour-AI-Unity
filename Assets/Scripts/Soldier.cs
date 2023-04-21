using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Transform visor;

    [Header("Soldier Basics")]
    [Tooltip("Health of the soldier")]
    [SerializeField] private float health;
    private const float MIN_HEALTH = 69;

    [Tooltip("Ammo of the soldier")]
    [SerializeField] private int ammo;
    private const int MAX_AMMO = 30;

    [Tooltip("Damage, it does damage")]
    [SerializeField] private float damage;

    [Header("Leader")]
    [Tooltip("Determines if the current soldier is the leader of the team")]
    [SerializeField] private bool isLeader;

    [Header("Sight and Awareness")]
    [Tooltip("The range of sight for the soldier")]
    public float sightRange;

    [Tooltip("The angle for the Field of View of the soldier")]
    public float sightAngle; // blue

    [Tooltip("Keeps track of surrounding objects that can be used as covers")]
    public float surroundingAwarenessRange; // yellow, keeps awareness of surrounding covers

    [Tooltip("The distance that the soldier will stay away from ally soldiers")]
    public float keepDistance; // black

    [Tooltip("The distance that the soldier will not go beyond from the Leader")]
    public float followDistance; // green

    [Tooltip("The distance for the soldier to be able to start shooting")]
    public float shootDistance; // red

    [Header("Detection Layers")]
    [Tooltip("Layer that determines if soldier is friendly")]
    [SerializeField] private LayerMask allyLayer;

    [Tooltip("Layer that determines if soldier is an enemy")]
    [SerializeField] private LayerMask enemyLayer;

    [Tooltip("Layer that determines if object can be used as cover")]
    [SerializeField] private LayerMask coverLayer;

    [Header("Tracking")]
    [Tooltip("Keeps track of objects that can be used as covers")]
    [SerializeField] private List<Transform> coverObjects;

    [Tooltip("Keeps track of spotted enemies")]
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

    // TODO: add these functions
    // Reload
    // Attack
    // WalkAround
    // Chase
    // TakeCover

    public bool HasBeenUnalived() 
    {
        return health <= 0;
    }

    public bool IsLowHealth()
    {
        return health <= MIN_HEALTH;
    }

    public void Damage(float damage)
    {
        if (damage <= 0) return;

        health -= damage;
        // probably can add other things here
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;

        health += amount;
        // probably can add other things here
    }

    public int GetCurrentAmmo()
    {
        return ammo;
    }

    public float GetCurrentHealth()
    {
        return health;
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
