using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyInteractable : MonoBehaviour, IInteractable
{
    public void Interactable(GameObject interactor)
    {
        // Check if the player has the key
        if (interactor.TryGetComponent(out PlayerInventory playerInventory))
        {
            playerInventory.hasKey = true;
            Debug.Log("Player has the key");
            Destroy(gameObject);
        }
    }
}
