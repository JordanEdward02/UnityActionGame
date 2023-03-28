using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour, Object
{
    [Header("Graphics")]
    [SerializeField] private ParticleSystem sparks;
    [Header("Objects")]
    [SerializeField] private GameObject explosionPrefab;

    bool lit = false;
    Quaternion idleRotation = new Quaternion(0f, 0.5f, 0f, 1f);

    public bool CanBePicked()
    {
        return !lit;
    }

    public void Use(PlayerInteractions parent, float power)
    {
        // Throws the bomb in front of the player and sets the explosion countdown
        Vector3 parentPos = parent.objectTransform.position;
        Quaternion parentRot = parent.objectTransform.rotation;
        transform.position = parentPos + parent.objectTransform.up * 0.9f + parent.objectTransform.forward * 1f;
        transform.rotation = parentRot;
        gameObject.GetComponent<SphereCollider>().enabled = true;
        Rigidbody rb;
        if (!(rb = gameObject.GetComponent<Rigidbody>()))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.AddForce(transform.forward * power, ForceMode.Impulse);
        parent.heldObject.ObjectDisconnect();
        if (lit)
            return;
        StartCoroutine(Explode());
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
        transform.position = parentPos + parent.objectTransform.up*0.65f + parent.objectTransform.right*0.3f;
        transform.rotation = parentRot * idleRotation;
    }

    IEnumerator Explode()
    {
        // Explodes the bomb after 3 seconds by destroying this object and spawning an explosion object at the current location. 
        lit = true;
        sparks.Play();
        yield return new WaitForSeconds(3);
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
