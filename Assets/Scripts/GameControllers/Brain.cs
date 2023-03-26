using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerPrefab;
    public Transform playerSpawn;
    void Awake()
    {
        GameObject player = GameObject.Find("Player(Clone)");
        if (player == null)
        {
            Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
        }
        else
        {
            player.transform.position = playerSpawn.position;
            player.transform.rotation = playerSpawn.rotation;
        }
    }
}
