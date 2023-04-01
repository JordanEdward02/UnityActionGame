using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Single use objects should have Sphere Colliders, otherwise additional behaviour needs adding to the PlayerInteractions class
// to remove the collider upon collision to pick up the object
public class SingleUseObjectController : MonoBehaviour, Object
{
    public ParticleSystem BreakEffect;
    public GameObject RockBody;
    [HideInInspector] public event System.Action CustomDestroy;

    bool thrown = false;
    Quaternion idleRotation = new Quaternion(0f, 0.5f, 0f, 1f);


    public bool CanBePicked()
    {
        return true;
    }
    public void Use(PlayerInteractions parent, float power)
    {
        Vector3 parentPos = parent.objectTransform.position;
        Quaternion parentRot = parent.objectTransform.rotation;
        transform.position = parentPos + parent.objectTransform.up * 0.9f + parent.objectTransform.forward * 0.5f;
        transform.rotation = parentRot;
        gameObject.GetComponent<SphereCollider>().enabled = true;
        Rigidbody rb;
        if (!(rb = gameObject.GetComponent<Rigidbody>()))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.AddForce(transform.forward * power, ForceMode.Impulse);
        parent.heldObject.ObjectDisconnect();
        thrown = true;
    }

    public void Drop()
    {
        gameObject.transform.position += transform.forward * 1f;
        gameObject.GetComponent<SphereCollider>().enabled = true;
        Rigidbody rb;
        if (!(rb = gameObject.GetComponent<Rigidbody>()))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }
    public void UpdateObject(PlayerInteractions parent)
    {
        // Moves the object into the player's view. Also controls the interactions such as the throw and block and detaches is from the player.
        Vector3 parentPos = parent.objectTransform.position;
        Quaternion parentRot = parent.objectTransform.rotation;
        transform.position = parentPos + parent.objectTransform.up * 0.65f + parent.objectTransform.right * 0.3f;
        transform.rotation = parentRot * idleRotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (thrown)
            StartCoroutine(Break());
    }

    IEnumerator Break()
    {
        //Breaks the rock after 1 second and shows the break effect. Stops all movement and collisions when this happens
        Destroy(RockBody);
        Destroy(gameObject.GetComponent<SphereCollider>());
        Rigidbody rb;
        if (!(rb = gameObject.GetComponent<Rigidbody>()))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.velocity = new Vector3(0,0,0);
        BreakEffect.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // Will tell the spawner the object is destroyed if this function is set
        CustomDestroy?.Invoke();
    }
    public GameObject GetObject()
    {
        try
        {
            return gameObject;
        }
        catch
        {
            return null;
        }
    }
}
