using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Brain5 : Brain
{
    int archerCount = 0;
    int killedArchers = 0;

    [Header("KeyDrop")]
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private int killsRequiredForKeySpawn;

    private bool keySpawned = false;


    private void Update()
    {
        ArcherController[] Archers = FindObjectsOfType<ArcherController>();
        int newCount = Archers.Length;
        if (newCount > archerCount)
            archerCount = newCount;
        if (newCount < archerCount)
        {
            ++killedArchers;
            Debug.Log("Killed Archers: " + killedArchers);
            archerCount = newCount;
        }


        // Adds the key to a random archer once enough have been killed
        if (!keySpawned && killedArchers > killsRequiredForKeySpawn)
        {
            keySpawned = true;
            ArcherController Archer = Archers[Random.Range(0, Archers.Length - 1)];
            LootDropper loot = Archer.gameObject.AddComponent<LootDropper>();
            loot.AddLoot(keyPrefab);
            Archer.lootShine.Play();
        }
    }
}
