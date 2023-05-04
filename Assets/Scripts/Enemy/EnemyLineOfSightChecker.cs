using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public Soldier soldier;
    public SphereCollider Collider;
    public float FieldOfView = 90f;
    public LayerMask LineOfSightLayers;

    public delegate void GainSightEvent(Transform Target);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(Transform Target);
    public LoseSightEvent OnLoseSight;

    private Transform DetectedEnemy;
    private Coroutine CheckForLineOfSightCoroutine;

    private void Awake()
    {
        soldier = gameObject.GetComponentInParent<Soldier>();
        Collider = GetComponent<SphereCollider>();
        DetectedEnemy = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        DetectedEnemy = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        OnLoseSight?.Invoke(other.transform);
        if (CheckForLineOfSightCoroutine != null)
        {
            StopCoroutine(CheckForLineOfSightCoroutine);
        }

        DetectedEnemy = null;
    }

    private void Hide()
    {
        if (DetectedEnemy == null) {
            return;
        }

        if (soldier.IsLowHealth()) {
            if (!CheckLineOfSight(DetectedEnemy))
            {
                CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(DetectedEnemy));
            }
        }
    }

    private bool CheckLineOfSight(Transform Target)
    {
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, direction);
        if (dotProduct >= Mathf.Cos(FieldOfView))
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Collider.radius, LineOfSightLayers))
            {
                OnGainSight?.Invoke(Target);
                return true;
            }
        }

        return false;
    }

    private IEnumerator CheckForLineOfSight(Transform Target)
    {
        WaitForSeconds Wait = new WaitForSeconds(0.5f);

        while(!CheckLineOfSight(Target))
        {
            yield return Wait;
        }
    }
}
