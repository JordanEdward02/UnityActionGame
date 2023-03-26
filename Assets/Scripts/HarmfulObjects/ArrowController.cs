using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour, HarmfulObject
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "BlockCollider")
        {
            if (gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = new Vector3();
                rb.angularVelocity = new Vector3();
                rb.AddForce(gameObject.transform.forward * -3f, ForceMode.Impulse);
            }
        }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))) {
            if (gameObject.TryGetComponent(out Rigidbody rb)) {
                Destroy(rb);
            }
            if (gameObject.TryGetComponent(out CapsuleCollider col))
            {
                Destroy(col);
            }
        }
    }
    */
}
