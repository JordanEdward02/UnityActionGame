using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShieldController : MonoBehaviour, Shield
{
    bool blocking = false;
    Quaternion idleRotation = new Quaternion(0f, -0.5f, 0f, 1f);

    [SerializeField] private Transform returnPoint;

    public bool IsBlocking()
    {
        return blocking;
    }
    public bool CanBePicked()
    {
        return true;
    }
    public void ToggleBlock()
    {
        blocking = !blocking;
    }

    public void Use(PlayerInteractions parent, float power)
    {
        Vector3 parentPos = parent.objectTransform.position;
        Quaternion parentRot = parent.objectTransform.rotation;
        // Throws the shield in front of the player and sets all the standard object variables
        transform.position = parentPos + parent.objectTransform.up*0.9f + parent.objectTransform.forward * 1f;
        transform.rotation = parentRot;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        Rigidbody rb;
        if (!(rb = gameObject.GetComponent<Rigidbody>()))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.AddForce(transform.forward * power, ForceMode.Impulse);
        gameObject.transform.Rotate(new Vector3(-90, 0, 0));
        parent.blockCollider.enabled = false;
        parent.heldShield.ShieldDisconnect();
        blocking = false;
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    public void Drop()
    {
        gameObject.transform.position += transform.forward*1f;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        Rigidbody rb;
        if (!(rb = gameObject.GetComponent<Rigidbody>()))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        blocking = false;

    }

    public void UpdateShield(PlayerInteractions parent)
    {
        // Moves the object into the player's view. Also controls the interactions such as the throw and block and detaches is from the player.
        Vector3 parentPos = parent.objectTransform.position;
        Quaternion parentRot = parent.objectTransform.rotation;
        parent.blockCollider.enabled = false;
        if (blocking)
        {
            // Enables the box collider in front of the player to stop the player being hit from the front when blocking.
            parent.blockCollider.enabled = true;
            transform.position = parentPos + parent.objectTransform.up * 0.3f;
            transform.rotation = parentRot;
        }
        else
        {
            transform.position = parentPos + parent.objectTransform.right*-0.4f + parent.objectTransform.up*0.3f;
            transform.rotation = parentRot * idleRotation;
        }
    }


    void FixedUpdate()
    {
        // Respawn when the shield falls out the map. Replace this with return to player after given time
        if (transform.position.y < -10.0)
        {
            transform.position = returnPoint.position;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3();
            gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3();
        }
    }
    public GameObject GetShield()
    {
        return gameObject;
    }
}
