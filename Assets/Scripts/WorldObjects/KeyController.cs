using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [Header("World Objects")]
    [SerializeField] GameObject lockedDoor;
    [SerializeField] Transform resetLocation;

    [Header("UI Interactions")]
    [SerializeField] int newSceneIndex;
    [SerializeField] string newString;
    [SerializeField] TooltipType newType;

    private void Start()
    {
        if (resetLocation == null)
        {
            GameObject location = new GameObject("KeyResetTransform");
            location.transform.position = transform.position;
            location.transform.rotation = transform.rotation;
            resetLocation = location.transform;
            LayerMask ground = Physics.AllLayers;
            ground &= ~(1 << LayerMask.GetMask("Enemy"));
            RaycastHit hit;
            if (Physics.Raycast(transform.position,
               -transform.up,
               out hit,
               10f, 
               ground))
            {
                location.transform.position = hit.point;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelEnd end = lockedDoor.AddComponent<LevelEnd>();
            end.newSceneIndex = newSceneIndex;
            TooltipHolder tooltip = lockedDoor.GetComponent<TooltipHolder>();
            tooltip.tooltip = newString;
            tooltip.type = newType;
            
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, resetLocation.position) > 50)
        {
            transform.position = resetLocation.position;
            transform.rotation = resetLocation.rotation;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector3();
            rb.angularVelocity = new Vector3();
        }
    }

    public bool SetVariables(GameObject door, int scene, string str, TooltipType type)
    {
        try
        {
            lockedDoor = door;
            newSceneIndex = scene;
            newString = str;
            newType = type;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
