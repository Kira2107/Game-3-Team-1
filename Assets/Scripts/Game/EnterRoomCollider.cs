using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomCollider : MonoBehaviour
{
    public List<NPCController> enemies;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach(var controller in enemies)
            {
                controller.InRoomWithPlayer = true;
            }
        }
    }
}
