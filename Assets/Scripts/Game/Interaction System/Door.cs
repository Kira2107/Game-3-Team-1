using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerInventory playerInventory))
        {
            if(playerInventory.hasKey)
            {
                Destroy(gameObject.transform.parent.gameObject);
                playerInventory.hasKey = false;
            }
        }
    }
}
