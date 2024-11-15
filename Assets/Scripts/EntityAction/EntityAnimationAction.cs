using System;
using UnityEngine;

public class EntityAnimationAction : EntityAction
{
    [SerializeField] private Animator animator;
    [SerializeField] private string actionAnimationName;
    [SerializeField] private float timeDuration;

    private Timer timer;
    private bool isPlayingAnimation;
    
    public override void StartAction()
    {
        base.StartAction();
        
        animator.CrossFade(actionAnimationName, timeDuration);

        timer = Timer.CreateTimer(timeDuration, true);
        timer.OnTick += OnTimerTick;
    }

    public override void EndAction()
    {
        base.EndAction();

        timer.OnTick -= OnTimerTick;
    }

    private void OnTimerTick()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(actionAnimationName))
            isPlayingAnimation = true;

        if (isPlayingAnimation)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(actionAnimationName))
            {
                isPlayingAnimation = false;
                
                EndAction();
            }
        }
    }
}
