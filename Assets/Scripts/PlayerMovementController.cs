using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Player))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference sneakAction;
    [SerializeField] InputActionReference sprintAction;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference pauseAction;

    [Space, Header("Settings")]
    public float baseSpeed = 1.0f;
    [SerializeField] float sneakModifier = 0.5f;
    [SerializeField] float sprintModifier = 2f;

    CharacterController controller;
    Player player;
    ColliderEvent interactionCaster;
    bool valid = false;

    List<Interactable> interactables;
    Interactable mainInteractable;

    private void Awake()
    {
        // Get controller reference
        TryGetComponent(out controller);
        TryGetComponent(out player);
        interactionCaster = GetComponentInChildren<ColliderEvent>();

        if (controller && player && interactionCaster)
        {
            valid = true;

            // Set up interaction caster
            interactionCaster.onTriggerEnter.AddListener(Cast_TriggerEnter);
            interactionCaster.onTriggerExit.AddListener(Cast_TriggerExit);
        }
    }

    private void FixedUpdate() // Handles movement control
    {
        if (valid)
        {
            // Get input vector
            Vector2 inputDirection = moveAction.action.ReadValue<Vector2>();

            // Don't move if very little input provided
            if (inputDirection.sqrMagnitude <= Mathf.Epsilon * Mathf.Epsilon) return;

            // Create movement vector from 2d
            Vector3 moveDirection = new(inputDirection.x, 0f, inputDirection.y);

            // Calculate speed
            float speed = baseSpeed;
            if (sneakAction.action.IsPressed()) speed *= sneakModifier;
            else if (sprintAction.action.IsPressed()) speed *= sprintModifier;
            
            // Move character
            controller.Move(speed * Time.fixedDeltaTime * moveDirection);
        }
    }

    private void Update() // Handles other input
    {
        if (valid)
        {
            // Check for interaction
            if (interactables.Count > 0)
            {
                float minDistSquared = Vector3.SqrMagnitude(interactionCaster.transform.position - interactables[0].transform.position);
                Interactable closest = interactables[0];

                // Get closest interactable
                for (int i = 1; i < interactables.Count; i++)
                {
                    float distSquared = Vector3.SqrMagnitude(interactionCaster.transform.position - interactables[i].transform.position);
                    if (distSquared < minDistSquared)
                    {
                        minDistSquared = distSquared;
                        closest = interactables[i];
                    }
                }

                // Set closest as main
                closest.SetInteraction(true);
                mainInteractable = closest;
            }
            else mainInteractable = null;

            // Interaction
            if (mainInteractable && interactAction.action.WasPressedThisFrame())
            {
                mainInteractable.Interact();
            }
        }
    }

    private void Cast_TriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Interactable interactable))
        {
            interactables.Add(interactable);
        }
    }

    private void Cast_TriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out Interactable interactable))
        {
            interactable.SetInteraction(false);
            interactables.Remove(interactable);
        }
    }
}
