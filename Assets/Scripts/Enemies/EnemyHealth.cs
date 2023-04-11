using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int hitPoints = 2;
    [SerializeField] float hitDelay = 0.5f;
    [SerializeField] ParticleSystem damageAffect;

    float hitTime;

    [HideInInspector] public bool customDeath = false;

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
                if (customDeath)
                    return;
                if (gameObject.TryGetComponent(out LootDropper loot))
                    loot.DropAllLoot();
                Destroy(gameObject);
                return;
            }
            StartCoroutine(DamageFlash());
            hitTime = Time.time;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
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
