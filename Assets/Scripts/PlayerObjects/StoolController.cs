using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoolController : MonoBehaviour, Object
{

    [HideInInspector] public event System.Action CustomDestroy;
    Quaternion idleRotation = new Quaternion(0f, 0.5f, 0f, 1f);

    public bool CanBePicked()
    {
        return true;
    }

    public void Use(PlayerInteractions parent, float power)
    {
        Vector3 parentPos = parent.objectTransform.position;
        Quaternion parentRot = parent.objectTransform.rotation;
        transform.position = parentPos + parent.objectTransform.up * 0.9f + parent.objectTransform.forward * 1f;
        transform.rotation = parentRot;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        Rigidbody rb;
        if (!(rb = gameObject.GetComponent<Rigidbody>()))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.AddForce(transform.forward * power, ForceMode.Impulse);
        gameObject.transform.Rotate(new Vector3(90, 0, 0));
        parent.heldObject.ObjectDisconnect();
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    public void Drop()
    {
        gameObject.transform.position += transform.forward * 1f;
        gameObject.GetComponent<BoxCollider>().enabled = true;
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
        transform.position = parentPos + parent.objectTransform.up * 0.3f + parent.objectTransform.right * 0.6f;
        transform.rotation = parentRot * idleRotation;
    }

    public void Update()
    {
        if (transform.position.y < -30)
            Destroy(gameObject);
    }

    void OnDestroy()
    {
        // Will tell the spawner the object is destroyed if this function is set
        CustomDestroy?.Invoke();
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
