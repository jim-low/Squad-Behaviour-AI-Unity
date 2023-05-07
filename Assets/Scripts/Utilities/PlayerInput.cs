using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, float.MaxValue, floorLayer)) {
                agent.SetDestination(hit.point);
            }
        }
    }
}
