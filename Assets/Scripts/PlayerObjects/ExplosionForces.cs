using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForces : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1);
    }
    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb;
        if (rb = other.gameObject.GetComponent<Rigidbody>()){
            Vector3 dir = other.gameObject.transform.position - transform.position;
            rb.AddForceAtPosition(dir.normalized, transform.position, ForceMode.Impulse);
        }
    }

}
