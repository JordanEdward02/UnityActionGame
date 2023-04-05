using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    [SerializeField] private List<GameObject> loot;

    LootDropper()
    {
        loot = new List<GameObject>();
    }

    public void AddLoot(GameObject newLoot)
    {
        loot.Add(newLoot);
    }

    public void DropAllLoot()
    {

        for (int i = 0; i < loot.Count; i++)
        {
            Instantiate(loot[i], transform.position, transform.rotation);
        }
        Destroy(this);
    }

}
