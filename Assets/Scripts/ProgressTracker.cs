using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressTracker : MonoBehaviour
{
    public TMP_Text progressText; // Assign in Inspector
    public int totalRooms; // Set in Inspector
    private int clearedRooms = 0;

    void Start()
    {
        UpdateProgressUI();
    }

    public void RoomCleared()
    {
        clearedRooms++; // Increment progress
        UpdateProgressUI();
    }

    private void UpdateProgressUI()
    {
        progressText.text = "Goal: Escape the Ship\nProgress: " + clearedRooms + " / " + totalRooms;
    }
}
