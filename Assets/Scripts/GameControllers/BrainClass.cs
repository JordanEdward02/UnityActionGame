using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainClass : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerPrefab;
    public Transform playerSpawn;
    void Awake()
    {
        if (GameObject.Find("Player(Clone)") == null)
        {
            Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
        }

    }
}
