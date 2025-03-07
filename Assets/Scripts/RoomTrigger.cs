using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private bool roomCleared = false; // Prevent double-counting

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !roomCleared)
        {
            roomCleared = true;
            FindObjectOfType<ProgressTracker>().RoomCleared(); // Update progress tracker
        }
    }
}
