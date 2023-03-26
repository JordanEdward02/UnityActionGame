using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    bool damaging = true;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (damaging)
        {
            if (collision.collider.gameObject.name == "BlockCollider")
            {
                if (gameObject.TryGetComponent(out Rigidbody rb))
                {
                    rb.velocity = new Vector3();
                    rb.angularVelocity = new Vector3();
                    rb.AddForce(gameObject.transform.forward * -3f, ForceMode.Impulse);
                }
                damaging = false;
            }
            else if (collision.gameObject.TryGetComponent(out PlayerGUI pGUI))
            {
                pGUI.Damage();
                damaging = false;
            }
        }
    }
}
