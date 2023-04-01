using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardWithKeyController : MonoBehaviour
{
    [Header("World Objects")]
    [SerializeField] private Rigidbody itemToPush;
    [SerializeField] private Transform positionToPushFrom;


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out DefaultObjectController stool))
        {
            if (itemToPush != null)
            {
                Vector3 pushDirection = itemToPush.transform.position - positionToPushFrom.position;
                itemToPush.AddForceAtPosition(pushDirection.normalized * 3f, positionToPushFrom.position, ForceMode.Impulse);
            }
        }
    }
}
