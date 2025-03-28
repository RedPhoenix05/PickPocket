using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class Interactable : MonoBehaviour
{
    protected TextMeshPro prompt;

    protected bool valid = false;
    protected bool interacting = false;

    protected virtual void Awake()
    {
        TryGetComponent(out prompt);

        if (prompt)
        {
            valid = true;

            prompt.enabled = false;
        }
    }

    public virtual void SetInteraction(bool interacting)
    {
        if (valid)
        {
            if (!this.interacting && interacting)
            {
                // On interaction enabled
                prompt.enabled = true;
            }
            else if (this.interacting && !interacting)
            {
                // On interaction disabled
                prompt.enabled = false;
            }

            this.interacting = interacting;
        }
    }

    public virtual void Interact()
    {

    }
}
