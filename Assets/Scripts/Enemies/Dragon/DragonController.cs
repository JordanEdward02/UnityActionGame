using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{

    // Animation variables
    bool attacking;
    bool dead; 
    int attackingKey = Animator.StringToHash("attacking");
    int deadKey = Animator.StringToHash("dead");

    Animator anim;

    // Behaviour
    [Header("Behaviour")]
    [SerializeField] float attackDelay;
    [SerializeField] float attackDuration;

    bool inRange = false;
    float previousAttack;

    GameObject target;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        previousAttack = Time.time - attackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            if (!(target = GameObject.Find("Player(Clone)")))
                return;

        if (inRange && Time.time > previousAttack + attackDelay)
        {
            StartCoroutine(BreathFire());
            previousAttack = Time.time;
        }
        anim.SetBool(attackingKey, attacking);
        anim.SetBool(deadKey, dead);
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
        attacking = true;
        yield return new WaitForSeconds(attackDuration);
        attacking = false;
    }
}
