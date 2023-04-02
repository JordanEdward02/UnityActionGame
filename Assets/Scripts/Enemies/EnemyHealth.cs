using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int hitPoints = 2;
    [SerializeField] float hitDelay = 1;

    float hitTime;

    public void Start()
    {
        hitTime = Time.time;
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
        MeshRenderer rend = gameObject.GetComponentInChildren<MeshRenderer>();
        for (float i = 0; i <= 0.5; i += Time.deltaTime)
        {
            Color col = rend.material.color;
            col.a -= i*100;
            rend.material.color = col;
            yield return null;
        }
        for (float i = 0; i <= 0.5; i += Time.deltaTime)
        {
            Color col = rend.material.color;
            col.a += i*100;
            rend.material.color = col;
            yield return null;
        }
    }
}
