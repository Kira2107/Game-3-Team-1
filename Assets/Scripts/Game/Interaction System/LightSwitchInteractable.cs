using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Light interactableLight;
    public NPCController npc;

    public void Interactable(GameObject interactor)
    {
        interactableLight.enabled = !interactableLight.enabled;


    }

    public void Disturbance (Vector3 disturbanceLocation)
    {
        if (npc != null)
        {
            npc.AlertNPC(disturbanceLocation);
        }
    }
}
