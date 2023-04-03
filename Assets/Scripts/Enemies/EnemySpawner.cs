using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyToSpawn;


    [Header("Behaviour")]
    [SerializeField] private Vector3 spawnRegionCentre;
    [SerializeField] private Vector3 spawnRegionSize;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(spawnRegionCentre, spawnRegionSize);
    }
}
