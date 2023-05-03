using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Transform visor;
    public healthBar soldierHealthBar;

    [Header("Soldier Basics")]
    [Tooltip("Health of the soldier")]
    [SerializeField] private float health = 100;
    private const float MIN_HEALTH = 30;
    private const float MAX_HEALTH = 100;

    [Tooltip("Ammo of the soldier")]
    [SerializeField] private int ammo = 30;
    private const int MAX_AMMO = 30;
    private const float RELOAD_TIME = 1.5f;
    private bool canShoot = true;
    private const float SHOOT_RECOIL = 0.25f;

    [Tooltip("Damage, it does damage")]
    [SerializeField] private float damage = 10;

    [Header("Leader")]
    [Tooltip("Determines if the current soldier is the leader of the team")]
    [SerializeField] private bool isLeader = false;

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

    void Start()
    {
        health = 30.0f;
        soldierHealthBar.SetMaxHealth(MAX_HEALTH);
        soldierHealthBar.SetHealth(health);
    }
    
    void Update()
    {
        Aim();
        Shoot();

        //when you want to reinitialize their health
        //soldierHealthBar.SetHealth(health);
    }

    private void DetectEnemies()
    {
        if (Physics.CheckSphere(transform.position, sightRange, enemyLayer))
        {
            Collider[] hitTargets = Physics.OverlapSphere(transform.position, sightRange, enemyLayer);
            foreach (var target in hitTargets)
            {
                Soldier enemy = target.gameObject.GetComponent<Soldier>();
                Vector3 soldierPos = enemy.transform.position - transform.position;
                float angle = Mathf.Abs(Vector3.Angle(soldierPos, transform.forward));

                if (angle <= sightAngle)
                {
                    enemy.SetDetection(true);
                    enemiesSpotted.Add(enemy.transform);
                }
            }
        }
    }


    // TODO: add these functions
    // Aim (aim closest enemy or enemy that is not being targeted?)
    // 

    private void Aim()
    {
        // get all colliders within sphere
        Collider[] enemies = Physics.OverlapSphere(transform.position, shootDistance, enemyLayer);
        Transform nearestEnemy = null;
        float prevDistance = 0;
        foreach (Collider enemy in enemies) {
            // find closest one
            if (prevDistance == 0) {
                nearestEnemy = enemy.transform;
                prevDistance = Vector3.Distance(enemy.transform.position, transform.position);
            }
            else {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < prevDistance) {
                    nearestEnemy = enemy.transform;
                }
                prevDistance = distance;
            }
        }

        // lerp towards it
        if (nearestEnemy != null) {
            Quaternion toRotation = Quaternion.LookRotation(nearestEnemy.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 5f * Time.deltaTime);
        }
    }

    private void Shoot()
    {
        // detect if enemy is within line of sight
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, shootDistance);

        if (hit.collider != null) {
            Debug.Log(hit.collider);
            Debug.Log(hit.collider.tag);
        }

        if (hit.collider != null && hit.collider.tag == "Enemy" && canShoot && ammo > 0)
        {
            hit.collider.GetComponent<Soldier>().Damage(damage);
            StartCoroutine(Recoil());
        }

        if (ammo <= 0) {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Recoil()
    {
        canShoot = false;
        yield return new WaitForSeconds(SHOOT_RECOIL);
        canShoot = true;
    }

    private IEnumerator Reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(RELOAD_TIME);
        ammo = MAX_AMMO;
        canShoot = true;
    }

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
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;

        health += amount;
    }

    public int GetCurrentAmmo()
    {
        return ammo;
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public void SetCurrentHealth(int health)
    {
        this.health = health;
    }


    // this is just drawing a bunch of things, nothing special lmao
    void OnDrawGizmos()
    {
        if (isLeader)
        {
            Gizmos.DrawIcon(transform.position + (Vector3.up * 2f), "leader.png", true);
        }

        /* // field of view */
         Gizmos.color = Color.blue; 
         Gizmos.DrawRay(visor.position, Quaternion.AngleAxis(sightAngle, transform.up) * visor.forward * sightRange); 
         Gizmos.DrawRay(visor.position, Quaternion.AngleAxis(-sightAngle, transform.up) * visor.forward * sightRange); 

        /* // keep distance from other soldiers */
        /* Gizmos.color = Color.black; */
        /* Gizmos.DrawWireSphere(transform.position, keepDistance); */

        /* // surrounding awareness range */
        /* Gizmos.color = Color.yellow; */
        /* Gizmos.DrawWireSphere(transform.position, surroundingAwarenessRange); */

        Gizmos.color = canShoot ? Color.yellow : Color.red;
        Gizmos.DrawRay(visor.position, visor.transform.forward * shootDistance);

        /* Gizmos.color = Color.green; */
        /* Gizmos.DrawWireSphere(transform.position, followDistance); */
    }
}
