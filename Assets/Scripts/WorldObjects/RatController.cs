using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] DoubleWaypoint nextDestination;
    [SerializeField] int sightFov = 220;
    [Header("Animation")]
    [SerializeField] AnimationCurve wiggleCurve;
    [SerializeField] GameObject ratBody;
    [Header("Functionality")]
    [SerializeField] Transform backTransform;
    NavMeshAgent agent;

    private GameObject carryingObject;

    bool goingForward = true;
    float toggleTime;
    // Start is called before the first frame update
    void Start()
    {
        toggleTime = Time.time;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = nextDestination.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        ratBody.transform.RotateAround(transform.forward, (wiggleCurve.Evaluate(Time.time) - 0.5f)*0.1f);
        #pragma warning restore CS0618 // Type or member is obsolete
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (goingForward)
            {
                DoubleWaypoint nextWaypoint = nextDestination.next;
                nextDestination = nextWaypoint;
                agent.destination = nextDestination.transform.position;
            }
            else
            {
                DoubleWaypoint nextWaypoint = nextDestination.previous;
                nextDestination = nextWaypoint;
                agent.destination = nextDestination.transform.position;
            }
        }
        if (carryingObject != null)
        {
            carryingObject.transform.position = backTransform.position;
            carryingObject.transform.rotation = backTransform.rotation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time > toggleTime + 5)
            {
                Vector3 direction = other.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < sightFov * 0.5f)
                {
                    toggleTime = Time.time;
                    goingForward = !goingForward;
                    agent.ResetPath();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.TryGetComponent(out ShieldController shield))
        {
            carryingObject = shield.gameObject;
            Destroy(obj.GetComponent<Rigidbody>());
            if (obj.TryGetComponent(out BoxCollider box))
            {
                box.enabled = false;
            }
        }
        if (obj.TryGetComponent(out StoolController stool) && carryingObject != null)
        {
            carryingObject.AddComponent<Rigidbody>();
            if (carryingObject.TryGetComponent(out BoxCollider box))
            {
                box.enabled = true;
            }
            Destroy(gameObject);
        }
    }
}
