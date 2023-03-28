using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// This class just makes sure to put the player in the correct location upon starting the level.
// Other scenes are built to reuse the same 

public class Brain3 : Brain
{
    [Header("Guard Event")]
    [SerializeField] GameObject guardPrefab;
    [SerializeField] Transform guardSpawn;
    bool spawnable = true;
    bool guardSpawned = false;

    [Header("Monitor")]
    [SerializeField] GameObject shield;


    // Start is called before the first frame update
    void Start()
    {
        GameObject player;
        if (player = GameObject.Find("Player(Clone)"))
        {
            player.transform.position = playerSpawn.position;
            player.transform.rotation = playerSpawn.rotation;
        }
    }
    private void Update()
    {
        if (shield.scene != SceneManager.GetActiveScene() && spawnable)
        {
            StartCoroutine(SpawnGuard());
            spawnable = false;
        }
        if (guardSpawned)
        {
           // if (find)
        }
    }

    IEnumerator SpawnGuard()
    {
        yield return new WaitForSeconds(4);
        Instantiate(guardPrefab, guardSpawn.localPosition, guardSpawn.rotation);
        guardSpawned = true;
    }
}
