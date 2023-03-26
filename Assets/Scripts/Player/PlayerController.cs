using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    float snapshotForward = 0;
    float snapshotSideways = 0;
    float currentY = 0;
    Quaternion snapshotRotation = new Quaternion();

    [Header("Movement")]
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float walkSpeed = 6;
    [SerializeField] private float mouseSensitivity = 2;

    [Header("Player Parts")]
    [SerializeField] private PlayerInteractions playerInteractions;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Camera conrtols with mouse. Move whole player in horizontal and just camera in verticle (for collisions)
        float rotationX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotationX, 0);
        float rotationY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        if (rotationY + currentY > 90)
            rotationY = 0;
        if (rotationY + currentY < -90)
            rotationY = 0;
        currentY += rotationY;
        Camera.main.transform.Rotate(-rotationY, 0, 0);

        // Movement controls with WASD
        float speedForward = Input.GetAxis("Vertical") * walkSpeed;
        float speedSideways = Input.GetAxis("Horizontal") * walkSpeed;

        // Jump logic, and snapshot of current speed to maintain during the jump. This stop unrealistic movements while in the air
        if (Input.GetButton("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x,jumpHeight,rb.velocity.z);
        }
        // Actual movement of the character using the values
        Vector3 moveVelocity;
        if (IsGrounded())
        {
            moveVelocity = transform.rotation * new Vector3(speedSideways, 0 , speedForward);
            snapshotSideways = speedSideways;
            snapshotForward = speedForward;
            snapshotRotation = transform.rotation;
        }
        else
        {
            // Uses the snapshot of the velocity as well as a tiny amount from the current inputs
            moveVelocity = snapshotRotation * new Vector3(snapshotSideways, 0, snapshotForward) + transform.rotation * new Vector3(speedSideways/10, 0, speedForward/10);
            
        }
        if (playerInteractions.throwing || playerInteractions.heldShield.IsBlocking())
        {
            moveVelocity *= 0.2f;
        }
        moveVelocity.y = rb.velocity.y;
        rb.velocity = moveVelocity;
    }

    public void FixedUpdate()
    {
        // Every fixed update slow down the movement of the player's jump slightly to prevent them getting stuck in boxes and stuff
        snapshotForward *= 0.99f;
        snapshotSideways *= 0.99f;
    }

    public bool IsGrounded()
    {
        // Add enemy into a layer and remove this from the mask, because it allows infinite jumps when in vision.

        // Checks if the user is touching the ground or not using an Overlapsphere.
        LayerMask otherLayers = Physics.AllLayers;
        otherLayers &= ~(1 << gameObject.layer);
        otherLayers &= ~(1 << LayerMask.NameToLayer("Enemy"));
        if (Physics.OverlapSphere(transform.position + new Vector3(0,-1,0), 0.4f, otherLayers).Length > 0)
        {
            return true;
        }
        return false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(0, -1, 0), 0.4f);
    }


}
