using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] Animator targetAnimator;          // The Animator on the red button
    [SerializeField] string animationTriggerName;      // The trigger name to play the press animation

    public Animator GetAnimator()
    {
        return targetAnimator;
    }

    public string GetAnimationTriggerName()
    {
        return animationTriggerName;
    }
}