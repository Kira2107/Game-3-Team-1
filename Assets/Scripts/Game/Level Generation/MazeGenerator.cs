using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject[] prefabPool;
    public int mazeLength = 5; // Number of prefabs to place
    private Vector3 currentPosition = Vector3.zero;
    private Quaternion currentRotation = Quaternion.identity;

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        GameObject previousPrefab =  prefabPool[0];

        for (int i = 0; i < mazeLength; i++)
        {
            // Select a random prefab
            GameObject selectedPrefab = prefabPool[Random.Range(0, prefabPool.Length)];
            GameObject newSegment = Instantiate(selectedPrefab, currentPosition, currentRotation);

            if (previousPrefab != null)
            {
                // Find Start and End points
                Transform previousEnd = previousPrefab.transform.Find("EndPoint");
                Transform newStart = newSegment.transform.Find("StartPoint");

                if (previousEnd != null && newStart != null)
                {
                    // Adjust position based on the previous segment's EndPoint
                    Vector3 offset = newStart.position - newSegment.transform.position;
                    newSegment.transform.position = previousEnd.position - offset;
                }
            }

            // Update reference for the next segment
            previousPrefab = newSegment;
        }
    }
}
