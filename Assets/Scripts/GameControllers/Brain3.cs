using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// This class just makes sure to put the player in the correct location upon starting the level.
// Other scenes are built to reuse the same 

public class Brain3 : Brain
{
    [Header("Monitor")]
    [SerializeField] GameObject itemToMonitor;

    [Header("Guard Event")]
    [SerializeField] GameObject guardPrefab;
    [SerializeField] Transform guardSpawn;
    [SerializeField] GameObject explosionPrefab;
    bool spawnable = true;
    bool guardSpawned = false;

    GameObject spawnedGuard;

    [Header("Level End Event")]
    [SerializeField] GameObject lockedDoor;
    [SerializeField] int newSceneIndex;
    [SerializeField] string newString;
    [SerializeField] TooltipType newType;


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
        if (itemToMonitor.scene != SceneManager.GetActiveScene() && spawnable)
        {
            StartCoroutine(SpawnGuard());
            spawnable = false;
        }
        if (guardSpawned)
        {
            if (spawnedGuard == null)
            {
                LevelEnd end = lockedDoor.AddComponent<LevelEnd>();
                end.newSceneIndex = newSceneIndex;
                TooltipHolder tooltip = lockedDoor.GetComponent<TooltipHolder>();
                tooltip.tooltip = newString;
                tooltip.type = newType;
                guardSpawned = false;
            }
        }
    }

    IEnumerator SpawnGuard()
    {
        yield return new WaitForSeconds(3);
        Instantiate(explosionPrefab, guardSpawn.localPosition, guardSpawn.rotation);
        yield return new WaitForSeconds(1);
        spawnedGuard = Instantiate(guardPrefab, guardSpawn.localPosition, guardSpawn.rotation);
        guardSpawned = true;
    }
}
