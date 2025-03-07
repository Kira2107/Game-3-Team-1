using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public GameObject interactionPrompt; // Assign the UI prompt in the Inspector

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false); // Hide prompt at start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
            interactionPrompt.SetActive(true); // Show the prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            interactionPrompt.SetActive(false); // Hide the prompt
        }
    }
}
