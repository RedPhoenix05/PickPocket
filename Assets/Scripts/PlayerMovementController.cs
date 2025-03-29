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
    [SerializeField] float gravity = -5f;

    [Space, Header("Animation")]
    [SerializeField] Transform playerModel;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip idleAnimation;
    [SerializeField] AnimationClip walkingAnimation;
    [SerializeField] AnimationClip sneakingAnimation;
    [SerializeField] AnimationClip runningAnimation;

    Vector3 lastPosition;

    CharacterController controller;
    Player player;
    ColliderEvent interactionCaster;
    bool valid = false;

    List<Interactable> interactables = new();
    Interactable mainInteractable;

    [HideInInspector] public bool disableMovement = false;

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

        lastPosition = playerModel.position;
    }

    private void FixedUpdate()
    {
        Vector3 forward = playerModel.position - lastPosition;
        if (forward.sqrMagnitude > Mathf.Epsilon)
        {
            playerModel.forward = forward;
        }
        lastPosition = playerModel.position;
    }

    private void Update() // Handles other input
    {
        if (valid)
        {
            UpdateMovement();
            UpdateInteraction();
        }
    }

    private void UpdateInteraction()
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
            mainInteractable.Interact(false);
        }
    }

    private void UpdateMovement()
    {
        if (!disableMovement)
        {
            // Get input vector
            Vector2 inputDirection = moveAction.action.ReadValue<Vector2>();

            if (inputDirection.sqrMagnitude < Mathf.Epsilon)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName(idleAnimation.name))
                {
                    animator.Play(idleAnimation.name, 0, 0f);
                }
            }
            else
            {
                if (sprintAction.action.IsPressed())
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(runningAnimation.name))
                    {
                        animator.Play(runningAnimation.name, 0, 0f);
                    }
                }
                else if (sneakAction.action.IsPressed())
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(sneakingAnimation.name))
                    {
                        animator.Play(sneakingAnimation.name, 0, 0f);
                    }
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(walkingAnimation.name))
                    {
                        animator.Play(walkingAnimation.name, 0, 0f);
                    }
                }
            }

            // Create movement vector from 2d
            Vector3 moveDirection = new(inputDirection.x, 0f, inputDirection.y);

            // Calculate speed
            float speed = baseSpeed;
            if (sneakAction.action.IsPressed()) speed *= sneakModifier;
            else if (sprintAction.action.IsPressed()) speed *= sprintModifier;

            // Calculate horizontal movement
            Vector3 horizontalFactor = speed * Time.deltaTime * moveDirection;

            // Convert global space to local space
            horizontalFactor = transform.TransformDirection(horizontalFactor);

            // Apply gravity
            Vector3 gravityFactor = Vector3.zero;
            if (!controller.isGrounded)
            {
                gravityFactor = -gravity * Time.deltaTime * Vector3.down;
            }

            // Move character
            controller.Move(horizontalFactor + gravityFactor);
        }
    }

    private void Cast_TriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Interactable interactable))
        {
            if (interactable.canInteract)
            {
                interactables.Add(interactable);
            }
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
