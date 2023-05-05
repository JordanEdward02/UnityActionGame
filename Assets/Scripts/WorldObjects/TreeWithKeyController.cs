using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWithKeyController : MonoBehaviour
{

    [Header("Objects")]
    [SerializeField] private GameObject key;

    [Header("Hits")]
    [SerializeField] private int hitsRequired = 3;
    [SerializeField] private ParticleSystem hitEffect;


    private void OnCollisionEnter(Collision collision)
    {
        hitEffect.Play();
        if (--hitsRequired == 0)
        {
            key.GetComponent<BoxCollider>().enabled = true;
            key.AddComponent<Rigidbody>();
        }
    }
}
