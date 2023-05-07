using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Transform visor;
    public HealthBar soldierHealthBar;

    // gun things
    [Header("Gun Things")]
    public GameObject bulletLinePrefab;
    public Transform firePoint;
    public GameObject bulletLine;

    [Header("Soldier Basics")]
    [Tooltip("Health of the soldier")]
    [SerializeField] private float health = 100;
    private const float MIN_HEALTH = 30;
    private const float MAX_HEALTH = 100;

    [Tooltip("Ammo of the soldier")]
    [SerializeField] public int ammo = 10;
    private const int MAX_AMMO = 10;
    private const float RELOAD_TIME = 1.5f;
    public bool canShoot = true;
    private const float SHOOT_RECOIL = 0.5f;
    private const float BULLET_APPEARANCE = 0.25f;

    [Tooltip("Damage, it does damage")]
    [SerializeField] public float damage = 10;

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
        visor = gameObject.transform.Find("Visor");
        firePoint = gameObject.transform.Find("Gun").gameObject.transform.Find("FirePoint");
        bulletLine = Object.Instantiate(bulletLinePrefab);
        bulletLine.SetActive(false);

        health = 100.0f;
        /* soldierHealthBar.SetMaxHealth(MAX_HEALTH); */
        /* soldierHealthBar.SetHealth(health); */
    }

    public void Recoil()
    {
        StartCoroutine(RecoilCoroutine());
    }

    private IEnumerator RecoilCoroutine()
    {
        canShoot = false;
        bulletLine.SetActive(true);
        yield return new WaitForSeconds(SHOOT_RECOIL);
        canShoot = true;
        bulletLine.SetActive(false);
    }

    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
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
        soldierHealthBar.SetHealth(health);
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;

        health += amount;
        soldierHealthBar.SetHealth(health);
    }
}
