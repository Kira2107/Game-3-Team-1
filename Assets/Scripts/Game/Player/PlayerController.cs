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
        // Move the player
        Vector3 moveDirection = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
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
        Gizmos.DrawWireSphere(transform.position, 2f);
    }

    private void TryInteract()
    {
        // Get all colliders in a sphere around the player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);

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
