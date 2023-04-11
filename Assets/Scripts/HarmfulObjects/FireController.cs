using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{

    void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent(out PlayerInteractions player))
        {
            player.Hit(gameObject.transform);
            Destroy(gameObject);
        }
    }
}
