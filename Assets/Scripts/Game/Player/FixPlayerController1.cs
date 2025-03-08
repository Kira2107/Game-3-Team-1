using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FixPlayerController : MonoBehaviour
{
    // Player's Rigidbody
    private Rigidbody playerRb;
    // Animator component (attached to the parent object)
    private Animator animator;

    // Player's movement speed
    [SerializeField] private float moveSpeed = 5f;
    
    // Reference to the camera (e.g., FreeCamera or Cinemachine FreeLook)
    public Transform cameraTransform;
    
    // Interaction range radius
    public float interactRadius = 1f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        if (playerRb == null)
        {
            UnityEngine.Debug.LogError("Rigidbody component not found. Please ensure the object has a Rigidbody attached!");
        }
        // Get the Animator component from the parent object
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            UnityEngine.Debug.LogError("Animator component not found. Please ensure the object has an Animator attached!");
        }
        if (cameraTransform == null)
        {
            UnityEngine.Debug.LogError("Camera Transform is not assigned. Please set it in the Inspector!");
        }
        // Hide the mouse cursor and lock it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Initially set to moving state (modify as needed)
        animator.SetBool("isMoving", true);
    }

    // Handle movement input
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        // Get the camera's forward and right directions, ignoring the Y-axis component
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();
        
        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Calculate player's movement direction based on camera's orientation
        Vector3 moveDirection = camForward * input.y + camRight * input.x;
        playerRb.velocity = moveDirection * moveSpeed;

        // Play moving animation when there is movement input
        if (animator != null)
        {
            // Set a threshold to avoid slight jitter being detected as movement
            bool isMoving = moveDirection.sqrMagnitude > 0.001f;
            animator.SetBool("isMoving", isMoving);
        }
    }

    // Update player's facing direction in LateUpdate so that it always faces the camera's horizontal direction
    private void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // Get the camera's horizontal rotation (around the Y-axis)
            float cameraYaw = cameraTransform.eulerAngles.y;
            // Set the player's rotation to match the camera's horizontal rotation
            transform.rotation = Quaternion.Euler(0, cameraYaw, 0);
        }
    }

    // Handle interaction input
    private void OnInteract()
    {
        TryInteract();
    }

    // Draw a sphere representing the interaction range in the Scene view for debugging purposes
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

    // Detect and execute interaction
    private void TryInteract()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interactable(gameObject);
            }
        }
    }
}
