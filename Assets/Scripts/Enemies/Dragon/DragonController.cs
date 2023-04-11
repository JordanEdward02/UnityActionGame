using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonController : MonoBehaviour
{

    // Animation variables
    bool attacking;
    bool dead;
    bool walking;
    int attackingKey = Animator.StringToHash("attacking");
    int deadKey = Animator.StringToHash("dead");
    int walkingKey = Animator.StringToHash("walking");
    int animationDuration = Animator.StringToHash("attackAnimationMultiplier");

    Animator anim;

    [Header("Behaviour")]
    [SerializeField] float attackDelay;
    [SerializeField] float attackDuration;
    [SerializeField] float fireDelay = 0.1f;

    [Header("Components")]
    [SerializeField] Transform fireSpawn;
    [SerializeField] GameObject firePrefab;
    [SerializeField] GameObject flamesGroup;

    bool inRange = false;
    float previousAttack;
    bool awake = false;

    GameObject target;
    NavMeshAgent agent;

    float lastFire;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        previousAttack = Time.time - attackDelay;
        lastFire = Time.time - fireDelay;
        anim.SetFloat(animationDuration, 2/attackDuration); // 2 is the actual duration in seconds of the animation
        StartCoroutine(WakeUp());
    }

    // Update is called once per frame
    void Update()
    {
        if (!awake)
            return;
        if (target == null)
            if (!(target = GameObject.Find("Player(Clone)")))
                return;

        // If the player is too far away, navAgent towards them
        if (!inRange)
        {
            if (!agent.pathPending)
            {
                agent.destination = target.transform.position;
            }
            walking = true;
        }

        if (inRange && !attacking)
        {
            // If we are within range but not looking at the player, try to rotate towards them
            Vector3 direction = target.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle > 3 || angle  < -3)
            {
                walking = true;
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 3 * Time.deltaTime);
            }
            else
                walking = false;
                // if criteria are met, start an attack. No other actions can occur, other than death, during the attack
                if (Time.time > previousAttack + attackDelay)
                {
                    agent.destination = transform.position;
                    StartCoroutine(BreathFire());
                }
        }


        anim.SetBool(walkingKey, walking);
        anim.SetBool(attackingKey, attacking);
        anim.SetBool(deadKey, dead);
    }

    private void FixedUpdate()
    {

        if (attacking && Time.time > lastFire + fireDelay)
        {
            lastFire = Time.time;
            GameObject Fire = Instantiate(firePrefab, fireSpawn.position, fireSpawn.rotation);
            Fire.transform.parent = flamesGroup.transform;
            if (Fire.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(fireSpawn.forward * 15f, ForceMode.Impulse);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            inRange = false;
    }

    IEnumerator BreathFire()
    {
        lastFire = Time.time + 0.5f;
        attacking = true;
        yield return new WaitForSeconds(attackDuration);
        attacking = false;
        previousAttack = Time.time;
    }

    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(6); // 6 seconds are needed on the wake up sequence before the dragon is actively persuing the player
        awake = true;
    }
}
