using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnDelay = 2.5f;

    [Header("Behaviour")]
    [SerializeField] private Vector3 spawnRegionCentre;
    [SerializeField] private Vector3 spawnRegionSize;

    [SerializeField] private Vector3 patrolRegionCentre;
    [SerializeField] private Vector3 patrolRegionSize;


    private List<GameObject> currentObjects = new();

    private void Start()
    {

        if (spawnRegionCentre != Vector3.zero && patrolRegionCentre != Vector3.zero)
            StartCoroutine(SpawnCycle());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(spawnRegionCentre, spawnRegionSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(patrolRegionCentre, patrolRegionSize);
    }

    IEnumerator SpawnCycle()
    {
        if (currentObjects.Count < spawnCount)
        {
            Vector3 spawnLocation = new Vector3(spawnRegionCentre.x + Random.Range(0, spawnRegionSize.x), 0, spawnRegionCentre.z + Random.Range(0, spawnRegionSize.z));
            GameObject enemy = Instantiate(enemyToSpawn, spawnLocation, transform.rotation);
            if (enemy.TryGetComponent(out ArcherController archer))
            {
                archer.seenLocation = new Vector3(patrolRegionCentre.x + Random.Range(0, patrolRegionSize.x), 0, patrolRegionCentre.z + Random.Range(0, patrolRegionSize.z));
            }
            currentObjects.Add(enemy);
        }
        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(SpawnCycle());
    }
}
