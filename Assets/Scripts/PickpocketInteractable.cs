using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class PickpocketInteractable : Interactable
{
    public virtual void DisablePickpocket()
    {
        // Disables prompting for further pickpocket
        prompt.enabled = false;

        if (TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }
    }
}
