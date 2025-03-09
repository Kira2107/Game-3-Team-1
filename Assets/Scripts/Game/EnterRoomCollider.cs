using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomCollider : MonoBehaviour
{
    public NPCController enemy;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy.InRoomWithPlayer = true;
        }
    }
}
