using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    void Update()
{
    RaycastHit hit;
    Vector3 origin = transform.position;
    Vector3 direction = transform.forward; // Shooting ray forward

    Debug.DrawRay(origin, direction * 10f, Color.red); // Visualize the ray

    if (Physics.Raycast(origin, direction, out hit, 10f))
    {
        Debug.Log("Hit: " + hit.collider.gameObject.name);
        Renderer rend = hit.collider.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = new Color(1, 1, 1, 0.3f); // Make semi-transparent
        }
    }
    else
    {
        Debug.Log("No hit detected.");
    }
}

}
