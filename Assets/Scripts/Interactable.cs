using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshPro))]
public class Interactable : MonoBehaviour
{
    public UnityEvent<bool> interactEvent;
    protected TextMeshPro prompt;

    protected bool valid = false;
    protected bool interacting = false;
    Transform mainCamera;

    [HideInInspector] public bool canInteract = true;

    protected virtual void Awake()
    {
        TryGetComponent(out prompt);

        if (prompt)
        {
            valid = true;

            prompt.enabled = false;
        }

        mainCamera = Camera.main.transform;
    }

    protected virtual void Update()
    {
        if (mainCamera)
        {
            transform.rotation = mainCamera.rotation;
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

    public virtual void Interact(bool forceInteraction)
    {
        if (canInteract)
        {
            interactEvent.Invoke(forceInteraction);
            Debug.Log("Interaction Event", this);
        }
    }

    public void EnableInteraction(float time)
    {
        Invoke(nameof(EnableInteraction), time);
    }

    public virtual void EnableInteraction()
    {
        canInteract = true;
    }
}
