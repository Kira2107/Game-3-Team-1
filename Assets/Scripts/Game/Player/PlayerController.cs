using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Player RigidBody
    private Rigidbody playerRb;

    //Player move speed
    [SerializeField] private float moveSpeed;
    public Transform cameraTransform;

    //interaction size
    public float interactRadius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        
        // Get camera forward direction, ignoring the Y axis (so movement remains on the XZ plane)
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        // Get camera right direction
        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Calculate movement direction based on camera orientation
        Vector3 moveDirection = camForward * input.y + camRight * input.x;

        // Apply velocity
        playerRb.velocity = moveDirection * moveSpeed;
    }

    private void OnInteract()
    {
        // Call the TryInteract method
        TryInteract();
    }

    private void OnDrawGizmos()
    {
        // Draw a sphere around the player
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

    private void TryInteract()
    {
        // Get all colliders in a sphere around the player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRadius);

        // Loop through all colliders
        foreach (var hitCollider in hitColliders)
        {
            // Check if the collider has an IInteractable component
            if (hitCollider.TryGetComponent(out IInteractable interactable))
            {
                // Call the Interact method on the IInteractable component
                interactable.Interactable(gameObject);
            }
        }
    }

}
