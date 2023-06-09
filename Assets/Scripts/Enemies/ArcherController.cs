using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherController : MonoBehaviour
{

    [Header("Ammo")]
    [SerializeField] GameObject shot;

    [Header("Components")]
    [SerializeField] Transform shotTransform;
    [SerializeField] GameObject head;
    [SerializeField] AudioSource shotSound;

    [Header("Behaviour")]
    [SerializeField] int sightFov = 120;
    [SerializeField] float attackDelay = 5;
    [SerializeField] float shotSpeed = 3f;
    [SerializeField] AnimationCurve idleLookAround;
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
                shotSound.Play();
                GameObject arrow = Instantiate(shot, shotTransform.position, shotTransform.rotation);
                if (arrow.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddForce(shotTransform.forward * shotSpeed, ForceMode.Impulse);
                }
                shotTime = Time.time;
            }
        }
        // If not looking/attacking the player, goes to where they were last seen
        if (!lookingAtPlayer)
        {
            if (!agent.pathPending)
                if (seenLocation != Vector3.zero)
                    agent.destination = seenLocation;

            // When walking towards the last seen location, align the position of the enemy head.
            if (seenLocation != Vector3.zero && Vector3.Distance(transform.position, seenLocation) > 5f)
            {
                Vector3 headForward = seenLocation;
                head.transform.LookAt(headForward, Vector3.up);
            }

            // If not moving, the enemy looks around
            if (agent.velocity == Vector3.zero)
            {
                head.transform.Rotate(new Vector3(0, (idleLookAround.Evaluate(Time.time) - 0.5f) * 3f, 0));
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lookingAtPlayer = false;
            Vector3 direction = other.transform.position - head.transform.position;
            float angle = Vector3.Angle(direction, head.transform.forward);
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
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    head.transform.LookAt(Camera.main.transform, Vector3.up);
                    lookingAtPlayer = true;
                    seenLocation = Vector3.zero;
                    agent.destination = transform.position;
                    engaged = true;
                }
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
            Gizmos.DrawLine(transform.position, fov * (GetComponent<SphereCollider>().radius * head.transform.forward) + transform.position);
            Gizmos.DrawLine(transform.position, fovneg * (GetComponent<SphereCollider>().radius * head.transform.forward) + transform.position);

            // When hit alert range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 15);
        }
    }

}