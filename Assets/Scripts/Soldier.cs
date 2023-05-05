using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Transform visor;
    public HealthBar soldierHealthBar;

    private Soldier target;

    // gun things
    [Header("Gun Things")]
    public GameObject bulletLinePrefab;
    public Transform firePoint;
    private GameObject bulletLine;

    [Header("Soldier Basics")]
    [Tooltip("Health of the soldier")]
    [SerializeField] private float health = 100;
    private const float MIN_HEALTH = 30;
    private const float MAX_HEALTH = 100;

    [Tooltip("Ammo of the soldier")]
    [SerializeField] private int ammo = 10;
    private const int MAX_AMMO = 10;
    private const float RELOAD_TIME = 1.5f;
    private bool canShoot = true;
    private const float SHOOT_RECOIL = 0.5f;
    private const float BULLET_APPEARANCE = 0.25f;

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
        firePoint = gameObject.transform.Find("Gun").gameObject.transform.Find("FirePoint");
        bulletLine = Instantiate(bulletLinePrefab);
        bulletLine.SetActive(false);

        health = 100.0f;
        soldierHealthBar.SetMaxHealth(MAX_HEALTH);
        soldierHealthBar.SetHealth(health);
    }
    
    void Update()
    {
        DetectEnemy();
        Aim();
        Shoot();
    }

    private bool DetectEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, sightRange, enemyLayer);

        float prevDistance = 0;
        Transform closestEnemy = null;
        foreach (Collider enemy in enemies) {
            if (closestEnemy == null) {
                closestEnemy = enemy.transform;
                prevDistance = Vector3.Distance(transform.position, enemy.transform.position);
            }
            else {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < prevDistance) {
                    closestEnemy = enemy.transform;
                    prevDistance = distance;
                }
            }
        }

        if (closestEnemy != null) {
            target = closestEnemy.GetComponent<Soldier>();
            return true;
        }
        return false;
    }

    private void Aim()
    {
        if (target == null) {
            return;
        }

        Vector3 lookDirection = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 2f * Time.deltaTime);
    }

    private bool Shoot()
    {
        if (target == null) {
            return false;
        }

        if (ammo <= 0) {
            if (canShoot) {
                StartCoroutine(Reload());
            }
            return false;
        }

        RaycastHit hit;
        bool hitEnemy = Physics.Raycast(transform.position, transform.forward, out hit, shootDistance);
        if (!hitEnemy || hit.collider.tag == "Cover") {
            return false;
        }

        Physics.Raycast(transform.position, transform.forward, out hit, shootDistance, enemyLayer);
        if (hit.collider != null && canShoot) {
            hit.collider.gameObject.GetComponent<Soldier>().Damage(damage);

            Vector3 oriScale = bulletLine.transform.localScale;
            float distance = Vector3.Distance(hit.collider.transform.position, firePoint.position);
            Quaternion bulletRotation = Quaternion.LookRotation(hit.collider.transform.position - firePoint.position);

            bulletLine.transform.position = firePoint.transform.position;
            bulletLine.transform.rotation = bulletRotation;
            bulletLine.transform.localScale = new Vector3(oriScale.x, oriScale.y, distance);

            ammo -= 1;
            StartCoroutine(Recoil());
        }
        return true;
    }

    private IEnumerator Recoil()
    {
        canShoot = false;
        bulletLine.SetActive(true);
        yield return new WaitForSeconds(SHOOT_RECOIL);
        canShoot = true;
        bulletLine.SetActive(false);
    }

    private IEnumerator Reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(RELOAD_TIME);
        ammo = MAX_AMMO;
        canShoot = true;
    }

    public bool Unalived() 
    {
        if (health <= 0) {
            // do some dying here
            return true;
        }

        return false;
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

        /* Gizmos.color = canShoot ? Color.yellow : Color.red; */
        /* Gizmos.DrawRay(visor.position, visor.transform.forward * shootDistance); */

        /* Gizmos.color = Color.green; */
        /* Gizmos.DrawWireSphere(transform.position, followDistance); */
    }
}
