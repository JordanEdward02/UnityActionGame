using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Player Objects")]
    public Transform objectTransform;
    public BoxCollider blockCollider;

    [Header("Gameplay Objects")]
    [SerializeField] private PlayerGUI gui;

    [HideInInspector] public PlayerObject heldObject;
    [HideInInspector] public ShieldObject heldShield;

    private float defaultFoV = 70f;

    [HideInInspector] public bool throwing = false;
    [HideInInspector] public float power = 5f;

    void Start()
    {
        heldObject = new PlayerObject(this);
        heldShield = new ShieldObject(this);
    }

    void Update()
    {
        // Controls for object interactions with mouse buttons
        if (Input.GetMouseButton(0) && (heldObject.HoldingObject() || heldShield.HoldingShield()))
        {
            throwing = true;
            if (power < 20f)
            {
                power += 10f*Time.deltaTime;
            }
            Camera.main.fieldOfView = defaultFoV - (power-5f);
        }
        else
        {
            if (throwing)
            {
                throwing = false;
                if (heldObject.HoldingObject())
                    heldObject.Use(power);
                else
                    heldShield.Use(power);
                Camera.main.fieldOfView = defaultFoV;
                power = 5f;
            }
        }

        if (heldShield.HoldingShield())
        {
            if (Input.GetMouseButtonDown(1)) heldShield.ToggleBlock();
            if (Input.GetMouseButtonUp(1) && heldShield.IsBlocking()) heldShield.ToggleBlock();
        }

        if (Input.GetKey(KeyCode.E))
            playerInteract();
        if (Input.GetKey(KeyCode.F))
            heldObject.Drop();
    }
    void FixedUpdate()
    {
        heldObject.Update();
        heldShield.UpdateShield();
    }
    public void playerInteract()
    {
        // Allows the player to interact with the exit level objects.
        RaycastHit hit;
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hit) && hit.distance < 6)
        {
            if (hit.collider.gameObject.TryGetComponent(out LevelEnd end))
            {
                StartCoroutine(gui.FadeOut(end.newSceneIndex));
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        // Tries to pick up an object when we walk on a playerobject.
        // Then removes the rigidbody and colliders to stop them from interacting with the player while being held

        // Ask if there is a better way to do this... Seems bad 
        GameObject obj = collision.gameObject;
        if (obj.GetComponent<Object>() != null)
        {
            if (heldObject.PickUp(obj))
            {
                Destroy(obj.GetComponent<Rigidbody>());
                if (obj.GetComponent<BombController>())
                {
                    obj.GetComponent<SphereCollider>().enabled = false;
                }
                else if (obj.GetComponent<RockController>())
                {
                    obj.GetComponent<SphereCollider>().enabled = false;
                }
                else if (obj.GetComponent<StoolController>())
                {
                    obj.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
        else if (obj.TryGetComponent(out Shield shield))
        {
            if (heldShield.PickUp(obj))
            {
                Destroy(obj.GetComponent<Rigidbody>());
                obj.GetComponent<BoxCollider>().enabled = false;
            }
        }
        else if (obj.CompareTag("Collectible"))
        {
            gui.collectible.sprite = obj.GetComponent<CollectibleImage>().image;
            gui.collectible.enabled = true;
            Destroy(obj);
        }
    }
}
