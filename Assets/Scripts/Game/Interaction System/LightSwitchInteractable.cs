using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Light interactableLight;

    public void Interactable(GameObject interactor)
    {
        interactableLight.enabled = !interactableLight.enabled;
    }
}
