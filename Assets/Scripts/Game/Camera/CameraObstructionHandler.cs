using System.Collections.Generic;
using UnityEngine;

public class CameraObstructionHandler : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public LayerMask wallLayer; // Assign the wall layer in the Inspector

    private List<Renderer> obstructedWalls = new List<Renderer>(); // Track walls that became transparent
    private Dictionary<Renderer, List<Color>> originalColors = new Dictionary<Renderer, List<Color>>();

    void Update()
    {
        HandleWallTransparency();
    }

    void HandleWallTransparency()
    {
    // Cast a ray from the camera to the player
    Vector3 raydirection = player.position - transform.position;
    Debug.DrawRay(transform.position, raydirection, Color.red, 0.1f); // Draw the ray for debugging

    RaycastHit[] hits1 = Physics.RaycastAll(transform.position, raydirection, raydirection.magnitude, wallLayer);

    // Debug.Log("Raycast hit count: " + hits1.Length); // Log how many objects it hits

    foreach (RaycastHit hit in hits1)
    {
        // Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
    }

        // Reset previously transparent walls
        foreach (Renderer renderer in obstructedWalls)
        {
            if (renderer != null && originalColors.ContainsKey(renderer))
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    renderer.materials[i].color = originalColors[renderer][i];
                }
            }
        }

        obstructedWalls.Clear();
        originalColors.Clear();

        // Cast a ray from the camera to the player
        Vector3 direction = player.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, direction.magnitude, wallLayer);

        foreach (RaycastHit hit in hits)
        {
            Renderer wallRenderer = hit.collider.GetComponent<Renderer>();

            if (wallRenderer != null)
            {
                // Debug.Log("Changing transparency for: " + wallRenderer.gameObject.name);

                // Save original colors if not already stored
                if (!originalColors.ContainsKey(wallRenderer))
                {
                    originalColors[wallRenderer] = new List<Color>();
                    foreach (Material mat in wallRenderer.materials)
                    {
                        originalColors[wallRenderer].Add(mat.color);
                    }
                }

                // Set transparency
                foreach (Material mat in wallRenderer.materials)
                {
                    Color color = mat.color;
                    color.a = 0.3f; // 30% transparency
                    mat.color = color;
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                }

                obstructedWalls.Add(wallRenderer);
            }
        }
    }
}
