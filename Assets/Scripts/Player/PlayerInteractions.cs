using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Player Objects")]
    public Transform objectTransform;
    [SerializeField] AudioSource damageSound;

    [Header("Gameplay Objects")]
    [SerializeField] private PlayerGUI gui;
    [SerializeField] private LineRenderer throwArc;

    [Header("Tutorials")]
    [SerializeField] private GameObject objectTutorial;
    [SerializeField] private GameObject shieldTutorial;
    [SerializeField] private GameObject movementTutorial;
    [SerializeField] private GameObject pauseMenu;

    [HideInInspector] public PlayerObject heldObject;
    [HideInInspector] public ShieldObject heldShield;
    [HideInInspector] public bool blocking = false;

    private float defaultFoV = 70f;

    [HideInInspector] static public bool hasUsedObject = false;
    [HideInInspector] static public bool hasUsedShield = false;
    [HideInInspector] static public bool hasUsedMovement = false;

    [HideInInspector] public bool throwing = false;
    [HideInInspector] public float power = 5f;
    [HideInInspector] static public bool showArc = false;

    void Start()
    {
        heldObject = new PlayerObject(this);
        heldShield = new ShieldObject(this);
        if (!hasUsedMovement)
        {
            hasUsedMovement = true;
            StartCoroutine(ShowTutotial(movementTutorial));
        }
        throwArc.numCapVertices = 5;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        // Controls for object interactions with mouse buttons
        if (Input.GetMouseButton(0) && (heldObject.HoldingObject() || heldShield.HoldingShield()))
        {
            throwing = true;
            if (power < 20f)
            {
                power += 10f*Time.deltaTime;
            }
            // Draw in a line for the throwing arc. This should use the built in gravity, plus the power of the inpulse
            Vector3 force = Camera.main.transform.forward * power *1.1f;
            if (showArc)
            {
                throwArc.positionCount = 20;
                int i = 0;
                for (float t = 0; t < 2.0; t += 0.1f)
                {
                    throwArc.SetPosition(i, Camera.main.transform.position + force * t + Physics.gravity * t * t * 0.5f + (Camera.main.transform.right * 0.2f) + (Camera.main.transform.forward));
                    ++i;
                }
            }
            Camera.main.fieldOfView = defaultFoV - (power-5f);
        }
        else
        {
            throwArc.positionCount = 0;
            if (throwing)
            {
                throwing = false;
                if (heldObject.HoldingObject())
                    heldObject.Use(power);
                else
                {
                    heldShield.Use(power);
                    blocking = false;
                }
                Camera.main.fieldOfView = defaultFoV;
                power = 5f;
            }
        }
        gui.SetPowerBar((power - 5) / 15);
        blocking = Input.GetKey(KeyCode.Mouse1);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
        }
            

        if (Input.GetKey(KeyCode.E))
            playerInteract();
    }
    void FixedUpdate()
    {
        heldObject.Update();
        heldShield.UpdateShield();
    }
    void playerInteract()
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
        GameObject obj = collision.gameObject;
        
        if (obj.GetComponent<Object>() != null)
        {
            if (heldObject.PickUp(obj))
            {
                Destroy(obj.GetComponent<Rigidbody>());
                if (obj.GetComponent<SingleUseObjectController>())
                {
                    obj.GetComponent<SphereCollider>().enabled = false;
                }
                else if (obj.GetComponent<DefaultObjectController>())
                {
                    obj.GetComponent<BoxCollider>().enabled = false;
                    if (obj.name == "Candle(Clone)") { 
                        obj.GetComponentsInChildren<ParticleSystem>()[0].Play();
                    }
                }
                if (!hasUsedObject)
                {
                    hasUsedObject = true;
                    StartCoroutine(ShowTutotial(objectTutorial));
                }
            }
        }
        else if (obj.TryGetComponent(out Shield shield))
        {
            if (heldShield.PickUp(obj))
            {
                Destroy(obj.GetComponent<Rigidbody>());
                obj.GetComponent<BoxCollider>().enabled = false;

                if (!hasUsedShield)
                {
                    hasUsedShield = true;
                    StartCoroutine(ShowTutotial(shieldTutorial));
                }
            }
        }
        else if (obj.CompareTag("Collectible"))
        {
            gui.collectible.sprite = obj.GetComponent<CollectibleImage>().image;
            gui.collectible.enabled = true;
            Destroy(obj);
        }
    }

    IEnumerator ShowTutotial(GameObject obj)
    {
        // Shows the given tutorial object for 4 seconds. This displays the tooltip attatched temporarily
        obj.SetActive(true);
        yield return new WaitForSeconds(3);
        obj.SetActive(false);
    }

    private void OnDestroy()
    {
        heldObject.Destroy();
        heldShield.Destroy();
    }

    // If the player is blocking then they don't take damage, otherwise they do. Uses dot product to find out if the item is in front of the player
    public void Hit(Transform hitLocation)
    {
        float angle = Vector3.Dot(transform.forward.normalized, hitLocation.forward.normalized);
        if (angle < -0.3 && blocking)
            return;
        if (damageSound != null) damageSound.Play();
        gui.Damage();
    }
}
