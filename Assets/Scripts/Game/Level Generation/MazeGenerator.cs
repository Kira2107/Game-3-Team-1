using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public List<GameObject> prefabPool;
    private List<GameObject> prefabPoolCopy = new List<GameObject>();
    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject endRoom;
    // public int mazeLength = 5; // Number of prefabs to place
    private Vector3 currentPosition = Vector3.zero;
    private Quaternion currentRotation = Quaternion.identity;

    void Start()
    {
        for(int i =0; i<prefabPool.Count; i++)
        {
            prefabPoolCopy.Add(prefabPool[i]);
        }
        StartCoroutine(GenerateLevel());
    }

    private IEnumerator GenerateLevel()
    {
        yield return new WaitForSeconds(1f);
        GenerateMaze();
    }

    void GenerateMaze()
    {
        GameObject previousPrefab =  startRoom;

        for (int i = 0; i < prefabPool.Count; i++)
        {
            // Select a random prefab
            GameObject selectedPrefab = prefabPoolCopy[Random.Range(0, prefabPoolCopy.Count)];
            GameObject newSegment = Instantiate(selectedPrefab, currentPosition, currentRotation);
            prefabPoolCopy.Remove(selectedPrefab);

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

        // Place the end room
        GameObject endRoomInstance = Instantiate(endRoom, currentPosition, currentRotation);
        Transform endRoomStart = endRoomInstance.transform.Find("StartPoint");
        Transform previousEndRoom = previousPrefab.transform.Find("EndPoint");

        if (endRoomStart != null && previousEndRoom != null)
        {
            Vector3 offset = endRoomStart.position - endRoomInstance.transform.position;
            endRoomInstance.transform.position = previousEndRoom.position - offset;
        }
    }
}
