using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int hitPoints = 2;
    [SerializeField] float hitDelay = 0.5f;
    [SerializeField] ParticleSystem damageAffect;

    float hitTime;

    public void Start()
    {
        hitTime = Time.time - hitDelay;
    }

    public void takeDamage()
    {
        if (Time.time > hitTime + hitDelay)
        {
            if (--hitPoints <= 0)
            {
                Destroy(gameObject);
                return;
            }
            StartCoroutine(DamageFlash());
            hitTime = Time.time;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the enemy is hit with anything other than the player running into it, so either a thrown object or a deflected harmful object
        if (!collision.gameObject.CompareTag("Player"))
        {
            takeDamage();
            if (TryGetComponent(out SphereCollider col))
            {
                col.radius = 15;
                StartCoroutine(NormaliseViewDistance(col));
            }
        }
    }

    IEnumerator NormaliseViewDistance(SphereCollider col)
    {
        yield return new WaitForSeconds(3);
        col.radius = 9;
    }

    // Currently not working, look into changing this to play a single loop animation for getting hit instead.
    IEnumerator DamageFlash()
    {
        if (damageAffect != null) damageAffect.Play();
        yield return new WaitForSeconds(0.5f);
    }
}
