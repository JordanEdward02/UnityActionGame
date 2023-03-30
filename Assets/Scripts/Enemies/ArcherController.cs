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
    [SerializeField] int attackDelay = 5;

    bool lookingAtPlayer = false;
    Vector3 seenLocation;
    float shotTime;

    NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        shotTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if (shotTime + attackDelay < Time.time)
        {
            if (lookingAtPlayer)
            {
                GameObject arrow = Instantiate(shot, shotTransform.position, shotTransform.rotation);
                if (arrow.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddForce(shotTransform.forward * 8f, ForceMode.Impulse);
                }
                shotTime = Time.time;
            }
        }
        if (!lookingAtPlayer && !agent.pathPending)
        {
            if (seenLocation != Vector3.zero)
            {
                agent.destination = seenLocation;
            }
        }
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
                    head.transform.LookAt(other.transform, Vector3.up);
                    lookingAtPlayer = true;
                    seenLocation = Vector3.zero;
                    agent.destination = transform.position;
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            lookingAtPlayer = false;
            seenLocation = other.transform.position;
        }
    }
}
