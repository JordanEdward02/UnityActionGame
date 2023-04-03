using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnDelay = 1.5f;

    [Header("Behaviour")]
    [SerializeField] private Vector3 spawnRegionCentre;
    [SerializeField] private Vector3 spawnRegionSize;

    [SerializeField] private Vector3 patrolRegionCentre;
    [SerializeField] private Vector3 patrolRegionSize;


    private List<GameObject> currentObjects = new();

    private float previousSpawn;

    private void Start()
    {
        previousSpawn = Time.time - spawnDelay;
    }

    private void Update()
    {

        if (currentObjects.Count < spawnCount && Time.time > previousSpawn + spawnDelay)
        {
            SpawnEnemy();
            previousSpawn = Time.time;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(spawnRegionCentre, spawnRegionSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(patrolRegionCentre, patrolRegionSize);
    }

    void SpawnEnemy()
    {
        Vector3 spawnLocation = new Vector3(spawnRegionCentre.x + Random.Range(-spawnRegionSize.x/2, spawnRegionSize.x/2), 0, spawnRegionCentre.z + Random.Range(-spawnRegionSize.z/2, spawnRegionSize.z/2));
        GameObject enemy = Instantiate(enemyToSpawn, spawnLocation, transform.rotation);
        if (enemy.TryGetComponent(out ArcherController archer))
        {
            archer.seenLocation = new Vector3(patrolRegionCentre.x + Random.Range(-patrolRegionSize.x / 2, patrolRegionSize.x/2), 0, patrolRegionCentre.z + Random.Range(-patrolRegionSize.z / 2, patrolRegionSize.z/2));
            archer.CustomDestroy += ()=>{ currentObjects.Remove(enemy); };
        }
        currentObjects.Add(enemy);
    }
}
