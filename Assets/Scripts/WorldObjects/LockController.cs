using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    public Rigidbody doorHinge;

    void Start()
    {
        if (doorHinge != null)
        doorHinge.freezeRotation = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //The lock breaks if a rock hits it
        if (collision.gameObject.TryGetComponent(out SingleUseObjectController rock))
        {
            // NEED TO TURN THE HINGE WHEN WE DESTROYED WITH THE ROCK. USING THE HINGE.
            if (doorHinge != null)
            { 
                doorHinge.freezeRotation = false;
                doorHinge.isKinematic = false;
            }
            Destroy(gameObject);
        }
    }
}
