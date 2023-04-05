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
            if (obj.TryGetComponent(out SingleUseObjectController rockController))
            {
                obj.GetComponent<SingleUseObjectController>().CustomDestroy += () => currentObjects.Remove(obj);
                obj.hideFlags = HideFlags.HideInHierarchy;
            }
            if (obj.TryGetComponent(out DefaultObjectController stoolController))
            {
                obj.GetComponent<DefaultObjectController>().CustomDestroy += () => currentObjects.Remove(obj);
                obj.hideFlags = HideFlags.HideInHierarchy;
            }
            if (obj.TryGetComponent(out DefaultShieldController shield))
            {
                shield.returnPoint = transform;
            }
            obj.transform.localScale = newScale;
            currentObjects.Add(obj);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (objectToSpawn == null)
            return;
        switch (objectToSpawn.name)
        {
            case "Candle":
                Gizmos.DrawWireCube(transform.position, new Vector3(0.25f, 0.5f, 0.25f));
                break;
            case "Rock":
                Gizmos.DrawWireSphere(transform.position, 0.2f);
                break;
            case "Shield_01":
                Gizmos.DrawWireCube(transform.position, new Vector3(0.7f, 0.7f, 0.08f));
                break;
            case "Stool_01":
                Gizmos.DrawWireCube(transform.position, new Vector3(0.45f, 0.6f, 0.45f));
                break;

        }
    }
}
