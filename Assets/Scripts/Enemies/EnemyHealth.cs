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

    // ADD IN A VISUAL WHEN THE ENEMY TAKES DAMAGE, LIKE A RED FLASH ON THEM OR SOMETHING
    public void takeDamage()
    {
        if (Time.time > hitTime + hitDelay)
        {
            if (--hitPoints <= 0)
            {
                Destroy(gameObject);
                return;
            }
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
}
