using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum AIState
{
    idle = 0,
    wander = 1,
    walk = 2,
    chase = 3
}

public class AIController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected AIAnimator animationController;
    [SerializeField] protected Transform visionSource;
    [SerializeField, Range(0f, 180f)] protected float visionAngle = 90f;
    [SerializeField, Min(0f)] protected float visionDistance = 5f;

    [Space, Header("Interaction")]
    [SerializeField] protected Interactable interactable;
    [SerializeField] protected float minInteractionSuspicionDist = 1f;
    [SerializeField] protected float interactionSuspicionMin = 5f; // suspicion from seen interaction at max distance
    [SerializeField] protected float interactionSuspicionMax = 20f; // suspicion from seen interaction at min distance

    [Space, Header("Suspicion")]
    [SerializeField] protected TextMeshPro suspicionDisplay;
    [SerializeField, Min(0f)] protected float currentSuspicion = 0f;
    [SerializeField, Min(0f)] protected float maxSuspicion = 20f;
    [SerializeField, Min(1f)] protected float suspicionModifier = 1f;
    [SerializeField, Min(0f)] protected float calmDelay = 5f;
    [SerializeField, Min(0f)] protected float calmRate = 1f;
    [SerializeField] protected List<float> suspicionLocks = new();
    [HideInInspector] public UnityEvent<float> alertEvent;
    [HideInInspector] public UnityEvent<float> warnEvent;

    protected GameManager gameManager;
    protected Player player;
    bool calmCooldown = false;
    float minSuspicion = 0f;
    float minDotProduct;
    protected bool valid = false;

    protected virtual void Awake()
    {
        player = FindFirstObjectByType<Player>();
        gameManager = FindFirstObjectByType<GameManager>();

        if (player && gameManager && animationController && visionSource && suspicionDisplay)
        {
            valid = true;
        }

        if (interactable)
        {
            interactable.interactEvent.AddListener(Interact);
        }

        minSuspicion = currentSuspicion;

        // Set at beginning for cone casting
        minDotProduct = Mathf.Cos(Mathf.Deg2Rad * visionAngle / 2);

        // Remove bad values
        for (int i = 0; i < suspicionLocks.Count; i++)
        {
            if (suspicionLocks[i] <= 0 || suspicionLocks[i] > maxSuspicion)
            {
                suspicionLocks.RemoveAt(i--);
            }
        }
    }

    protected virtual void Update()
    {
        if (valid)
        {
            // Update suspicion level display
            float suspicionLevel = Mathf.Clamp01(currentSuspicion / maxSuspicion);
            suspicionDisplay.text = ((int)(suspicionLevel * 100)).ToString() + "%";
            suspicionDisplay.color = Color.Lerp(Color.green, Color.red, suspicionLevel);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (valid)
        {
            UpdateMinSuspicion();

            // AI is aware of the player
            if (currentSuspicion >= maxSuspicion)
            {
                Alert(currentSuspicion);
            }

            // Calming down after period of inactivity
            if (!calmCooldown)
            {
                CalmDown(calmRate * Time.fixedDeltaTime);
            }
        }
    }

    public virtual void Warn(float value)
    {
        currentSuspicion += value;
        calmCooldown = true;
        Invoke(nameof(DisableCalmCooldown), calmDelay);
        warnEvent.Invoke(value);
    }

    public virtual void Alert(float value)
    {
        calmCooldown = true;
        alertEvent.Invoke(value);

        gameManager.GameFail();
    }

    private void UpdateMinSuspicion()
    {
        // Sets new minimum suspicion level to highest locked level
        foreach (float value in suspicionLocks)
        {
            if (currentSuspicion >= value && minSuspicion < value)
            {
                minSuspicion = value;
            }
        }
    }

    private void DisableCalmCooldown()
    {
        calmCooldown = false;
    }

    public virtual void CalmDown(float value)
    {
        currentSuspicion = Mathf.Clamp(currentSuspicion - value, minSuspicion, maxSuspicion);
    }

    protected virtual bool PlayerVisionCast()
    {
        // Exit if player outside range
        if (Vector3.SqrMagnitude(visionSource.position - player.visionCheck.position) > visionDistance * visionDistance) return false;

        // Do math
        Vector3 playerVector = (player.visionCheck.position - visionSource.position).normalized;
        float dotProduct = Vector3.Dot(visionSource.forward, playerVector);

        // Check if player inside valid angle;
        return dotProduct >= minDotProduct;
    }


    protected virtual void Interact(bool forceVisible = false)
    {
        // increase suspicion if interaction done in vision
        if (forceVisible || PlayerVisionCast())
        {
            float playerDistSqr = Vector3.SqrMagnitude(player.visionCheck.position - visionSource.position);
            float minInteractionSuspicionDistSqr = minInteractionSuspicionDist * minInteractionSuspicionDist;
            float visionDistanceSqr = visionDistance * visionDistance;
            playerDistSqr = Mathf.Clamp(playerDistSqr, minInteractionSuspicionDistSqr, visionDistanceSqr);
            float value = Mathf.Lerp(interactionSuspicionMax, interactionSuspicionMin, playerDistSqr / visionDistanceSqr);

            Warn(value);
        }
    }
}
