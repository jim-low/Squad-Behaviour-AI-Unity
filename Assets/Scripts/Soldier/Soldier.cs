using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    protected Transform visor;
    protected HealthBar soldierHealthBar;

    // gun things
    [Header("Gun Things")]
    public GameObject bulletLinePrefab;
    public Transform firePoint;
    private GameObject bulletLine;
    private bool isLowHealth = false;

    // other things
    private SphereCollider lineOfSightCollider;

    [Header("Soldier Basics")]
    [Tooltip("Health of the soldier")]
    [SerializeField] private float health = 100f;
    private bool isHealing = false;
    private const float HEAL_DELAY = 1f;
    private const float HEAL_AMT = 3f;
    private const float MIN_HEALTH = 60f;
    private const float MAX_HEALTH = 100f;
    private bool isDied = false;

    [Tooltip("Ammo of the soldier")]
    [SerializeField] protected int ammo = 10;
    private const int MAX_AMMO = 10;
    private const float RELOAD_TIME = 1.5f;
    public bool canShoot = true;
    private const float SHOOT_RECOIL = 0.5f;
    private const float BULLET_FLASH_SECONDS = 0.075f;

    [Tooltip("Damage, it does damage")]
    [SerializeField] protected float damage = 10;

    [Header("Sight and Awareness")]
    [Tooltip("The range of sight for the soldier")]
    public float sightRange;

    [Tooltip("The angle for the Field of View of the soldier")]
    public float sightAngle; // blue

    [Tooltip("The distance for the soldier to be able to start shooting")]
    public float shootDistance; // red

    [Header("Detection Layers")]
    [Tooltip("Layer that determines if soldier is friendly")]
    [SerializeField] private LayerMask allyLayer;

    [Tooltip("Layer that determines if soldier is an enemy")]
    [SerializeField] private LayerMask enemyLayer;

    [Tooltip("Layer that determines if object can be used as cover")]
    [SerializeField] private LayerMask coverLayer;

    void Start()
    {
        visor = gameObject.transform.Find("Visor");
        soldierHealthBar = gameObject.transform.gameObject.transform.gameObject.transform.Find("HealthBarCanvas").gameObject.transform.gameObject.transform.Find("healthBar").GetComponent<HealthBar>();

        gameObject.transform.gameObject.transform.gameObject.transform.gameObject.transform.Find("HealthBarCanvas").GetComponent<BillBoard>().cam = GameObject.Find("Main Camera").transform.gameObject.transform.gameObject.transform.transform.transform.transform.gameObject.transform;

        lineOfSightCollider = gameObject.transform.Find("LineOfSightChecker").GetComponent<SphereCollider>();

        firePoint = gameObject.transform.Find("Gun").gameObject.transform.Find("FirePoint");
        bulletLine = Object.Instantiate(bulletLinePrefab);
        bulletLine.SetActive(false);

        health = 100.0f;
        //soldierHealthBar.SetMaxHealth(MAX_HEALTH);
        //soldierHealthBar.SetHealth(health);
    }

    public IEnumerator WaitSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void Shoot(Transform target)
    {
        if (!canShoot || isDied || Unalived()) {
            return;
        }

        canShoot = false;
        StartCoroutine(ShootCoroutine(target));
    }

    private IEnumerator ShootCoroutine(Transform target)
    {
        Vector3 oriScale = bulletLine.transform.localScale;
        float distance = Vector3.Distance(target.position, firePoint.position);
        Quaternion bulletRotation = Quaternion.LookRotation(target.position - firePoint.position);

        bulletLine.transform.position = firePoint.transform.position;
        bulletLine.transform.rotation = bulletRotation;
        bulletLine.transform.localScale = new Vector3(oriScale.x, oriScale.y, distance);

        target.GetComponent<Soldier>().Damage(damage);

        ammo -= 1;
        bulletLine.SetActive(true);
        yield return new WaitForSeconds(BULLET_FLASH_SECONDS);
        bulletLine.SetActive(false);

        yield return new WaitForSeconds(SHOOT_RECOIL);
        canShoot = true;
    }

    public void Reload()
    {
        if (!canShoot || isDied) {
            return;
        }

        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        canShoot = false;
        yield return new WaitForSeconds(RELOAD_TIME);
        ammo = MAX_AMMO;
        canShoot = true;
    }

    public void Hide(bool startHide)
    {
        lineOfSightCollider.enabled = startHide;
    }

    public bool Unalived() 
    {
        if (health <= 0) {
            return true;
        }

        return false;
    }

    public void DieLmao()
    {
        if (isDied) {
            return;
        }

        GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Force);

        enabled = false;
        GetComponent<SoldierBehaviorTree>().enabled = false; // disable behaviour tree cuz it has deathed
        GetComponent<NavMeshAgent>().enabled = false; // disable Nav Mesh AI cuz ded ppl no need move
        GetComponent<HideMovement>().enabled = false; // disable hide
        Hide(false); // disable hide
        canShoot = false; // disable attack
        transform.Rotate(0, 0, 90f); // turn it around
        isDied = true;
    }

    public void Reset()
    {
        enabled = true;
        GetComponent<SoldierBehaviorTree>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<HideMovement>().enabled = true;
        Hide(true);
        canShoot = true;
        transform.Rotate(0, 0, 0);
        isDied = false;
    }

    public bool IsLowHealth()
    {
        return isLowHealth;
    }

    public bool NeedReload()
    {
        return ammo <= 0;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void Damage(float damage)
    {
        if (damage <= 0) return;

        health -= damage;
        if (health <= MIN_HEALTH) {
            isLowHealth = true;
        }
        soldierHealthBar.SetHealth(health);
    }

    public void Heal()
    {
        if (isHealing || !isLowHealth || isDied) {
            return;
        }

        StartCoroutine(HealCoroutine());
    }

    private IEnumerator HealCoroutine()
    {
        if (isHealing) {
            yield return null;
        }

        health += HEAL_AMT;
        soldierHealthBar.SetHealth(health);
        isHealing = true;

        if (health >= (MAX_HEALTH * 0.8)) {
            isLowHealth = false;
        }

        if (health >= MAX_HEALTH) {
            health = MAX_HEALTH;
        }

        yield return new WaitForSeconds(HEAL_DELAY);
        isHealing = false;
        Heal();
    }

    public void setHealth(float health) {
        this.health = health;
    }

    public void setHealthBar(float health)
    {
        this.soldierHealthBar.SetHealth(health);
    }

    public float getHealth()
    {
        return health;
    }

    public float getMaxHealth()
    {
        return MAX_HEALTH;
    }

    public string GetEnemyLayer()
    {
        return LayerMask.LayerToName(Mathf.RoundToInt(Mathf.Log(enemyLayer.value, 2)));
    }
}
