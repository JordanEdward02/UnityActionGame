using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherController : MonoBehaviour
{

    [Header("Ammo")]
    [SerializeField] GameObject shot;

    [Header("Enemy Objects")]
    [SerializeField] Transform shotTransform;
    [SerializeField] GameObject head;

    [Header("Behaviour Parameters")]
    [SerializeField] int sightFov;
    [SerializeField] DoubleWaypoint nextDestination;

    NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 direction = other.transform.position - head.transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < sightFov * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + transform.up,
                   direction.normalized,
                   out hit,
                   GetComponent<SphereCollider>().radius))
                {
                    head.transform.LookAt(other.transform,Vector3.up);
                }
            }
            
        }
    }
}
