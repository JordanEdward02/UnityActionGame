using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int maxNumberOfObjects;
    [SerializeField] private Vector3 newScale = new Vector3(1,1,1);

    private List<GameObject> currentObjects = new();
    
    void Update()
    {
        // Spawns in objects if there are not enough.
        while (currentObjects.Count < maxNumberOfObjects)
        {
            GameObject obj = Instantiate(objectToSpawn, transform.position, transform.rotation);
            // Lamba function that will remove the object from the list if it is destoryed, so that we spawn a new one. This is called in the object OnDestory.
            // See RockController for more.
            if (obj.TryGetComponent(out RockController rockController))
            {
                obj.GetComponent<RockController>().CustomDestroy += () => currentObjects.Remove(obj);
            }
            if (obj.TryGetComponent(out StoolController stoolController))
            {
                obj.GetComponent<StoolController>().CustomDestroy += () => currentObjects.Remove(obj);
            }
            obj.transform.localScale = newScale;
            currentObjects.Add(obj);
        }
    }
}
