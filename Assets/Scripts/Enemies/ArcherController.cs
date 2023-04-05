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
    [SerializeField] int sightFov = 120;
    [SerializeField] float attackDelay = 5;
    public ParticleSystem lootShine;  

     
    [HideInInspector] public event System.Action CustomDestroy;

    bool lookingAtPlayer = false;
    bool engaged = false;
    float shotTime;

    [HideInInspector] public Vector3 seenLocation;

    NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        shotTime = Time.time;
    }

    void Update()
    {
        if (shotTime + attackDelay < Time.time)
        {
            if (lookingAtPlayer)
            {
                GameObject arrow = Instantiate(shot, shotTransform.position, shotTransform.rotation);
                if (arrow.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddForce(shotTransform.forward * 3f, ForceMode.Impulse);
                }
                shotTime = Time.time;
            }
        }
        // If not looking/attacking the player, goes to where they were last seen
        if (!lookingAtPlayer && !agent.pathPending)
        {
            if (seenLocation != Vector3.zero)
            {
                agent.destination = seenLocation;
            }
        }
        // When we are walking towards the last seen location, align the position of the enemy head.
        if (!lookingAtPlayer && seenLocation != Vector3.zero && Vector3.Distance(transform.position, seenLocation) > 5f)
            head.transform.LookAt(seenLocation, Vector3.up);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lookingAtPlayer = false;
            Vector3 direction = other.transform.position - head.transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            // If the player is not within the sight, the enemy attention is half as effective.
            float FoVRange = GetComponent<SphereCollider>().radius;
            if (angle > sightFov * 0.5f && !engaged)
                FoVRange /= 2;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.up,
               direction.normalized,
               out hit,
               FoVRange))
            {
                head.transform.LookAt(Camera.main.transform, Vector3.up);
                lookingAtPlayer = true;
                seenLocation = Vector3.zero;
                agent.destination = transform.position;
                engaged = true;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            lookingAtPlayer = false;
            engaged = false;
            seenLocation = other.transform.position;
        }
    }

    private void OnDestroy()
    {
        CustomDestroy?.Invoke();
    }


    public void OnDrawGizmos()
    {
        if (GetComponent<SphereCollider>() != null)
        {
            // Default backwards alert range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius / 2);

            // Default foward alert range
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
            Quaternion fov = Quaternion.AngleAxis(sightFov * 0.5f, Vector3.up);
            Quaternion fovneg = Quaternion.AngleAxis(sightFov * -0.5f, Vector3.up);
            Gizmos.DrawLine(transform.position, fov * (GetComponent<SphereCollider>().radius * transform.forward) + transform.position);
            Gizmos.DrawLine(transform.position, fovneg * (GetComponent<SphereCollider>().radius * transform.forward) + transform.position);

            // When hit alert range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 15);
        }
    }

}