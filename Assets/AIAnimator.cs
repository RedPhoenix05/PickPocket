using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AIState
{
    idle = 0,
    wander = 1,
    walk = 2,
    chase = 3
}

[RequireComponent(typeof(Animator))]
public class AIAnimator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0f)] protected float fadeTime = 0.25f;
    [SerializeField] protected AIState defaultAnimationState = AIState.idle;

    [Space, Header("Idle Animation")]
    [SerializeField] protected List<AnimationClip> idleAnimation;
    [SerializeField, Min(0)] protected int minIdleRepetitions = 0;
    [SerializeField, Min(0)] protected int maxIdleRepetitions = 0;

    [Space, Header("Walk Animation")]
    [SerializeField] protected List<AnimationClip> walkAnimation;
    [SerializeField, Min(0)] protected int minWalkRepetitions = 0;
    [SerializeField, Min(0)] protected int maxWalkRepetitions = 0;

    [Space, Header("Run Animation")]
    [SerializeField] protected List<AnimationClip> runAnimation;
    [SerializeField, Min(0)] protected int minRunRepetitions = 0;
    [SerializeField, Min(0)] protected int maxRunRepetitions = 0;

    protected Animator animator;
    protected List<AnimationClip> selectedClips = new();
    protected AnimationClip selectedClip;
    protected int currentRepetitions = 0;
    protected int targetRepetitions = 0;
    protected int minRepetitions = 0;
    protected int maxRepetitions = 0;
    bool valid = false;

    string currentState = string.Empty;

    protected virtual void Awake()
    {
        TryGetComponent(out animator);

        if (animator)
        {
            valid = true;
        }
    }

    protected virtual void Start()
    {
        if (valid)
        {
            SetAnimation(defaultAnimationState);
        }
    }

    protected virtual void Update()
    {
        if (valid)
        {
            // Continuously play a random animation based on the state
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !animator.IsInTransition(0))
            {
                // If we have completed the required number of repetitions, pick a new animation
                if (currentRepetitions >= targetRepetitions)
                {
                    PlayRandomAnimation(selectedClips);  // Change to the desired animation category
                    currentRepetitions = 0;

                    // Set random
                    targetRepetitions = Random.Range(minRepetitions, maxRepetitions + 1);
                }
                else
                {
                    // Keep repeating the current animation
                    currentRepetitions++;
                    animator.Play(currentState, 0, 0f);
                }
            }
        }
    }

    public virtual void SetAnimation(AIState state)
    {
        if (valid)
        {
            switch (state)
            {
                case AIState.idle:
                    selectedClips = idleAnimation;
                    minRepetitions = minIdleRepetitions;
                    maxRepetitions = maxIdleRepetitions;
                    break;
                case AIState.wander:
                case AIState.walk:
                    selectedClips = walkAnimation;
                    minRepetitions = minWalkRepetitions;
                    maxRepetitions = maxWalkRepetitions;
                    break;
                case AIState.chase:
                    selectedClips = runAnimation;
                    minRepetitions = minRunRepetitions;
                    maxRepetitions = maxRunRepetitions;
                    break;
            }

            if (maxRepetitions < minRepetitions) maxRepetitions = minRepetitions;

            // Play random
            PlayRandomAnimation(selectedClips);
        }
    }

    // Function to play a random animation from a specific category
    void PlayRandomAnimation(List<AnimationClip> clips)
    {
        if (valid)
        {
            selectedClip = clips[Random.Range(0, clips.Count)];

            // Set the current state to the name of the selected animation (or category)
            currentState = selectedClip.name;

            // Crossfade to the selected animation clip
            animator.CrossFade(selectedClip.name, fadeTime);
        }
    }
}
