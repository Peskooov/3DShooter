using System;
using UnityEngine;

[Serializable]
public class CharacterAnimatorParametersName
{
    public string NormalizeMovementX;
    public string NormalizeMovementY;
    public string ClimbInput;
    public string Climb;
    public string Sprint;
    public string Crouch;
    public string Aiming;
    public string Ground;
    public string Jump;
    public string GroundSpeed;
    public string DistanceToGround;
}

[Serializable]
public class AnimationCrossFadeParameters
{
    public string name;
    public float duration;
}

public class CharacterAnimatorState : MonoBehaviour
{
    [SerializeField] private float inputControlSpeed;
    
    [SerializeField] private CharacterController targetCharacterController;
    [SerializeField] private CharacterMovement targetCharacterMovement;
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private CharacterAnimatorParametersName animatorParametersName;

    [Header("Fades")] [Space(5)] 
    [SerializeField] private float minDistanceToGroundByFall;
    [SerializeField] private AnimationCrossFadeParameters fallFade;
    [SerializeField] private AnimationCrossFadeParameters jumpIdleFade;
    [SerializeField] private AnimationCrossFadeParameters jumpMoveFade;
    
    private Vector3 inputControl;

    private void LateUpdate()
    {
        Vector3 movementSpeed =
            transform.InverseTransformDirection(targetCharacterController
                .velocity); // Change Local to World transform position
        inputControl = Vector3.MoveTowards(inputControl, targetCharacterMovement.TargetDirectionControl,
            inputControlSpeed * Time.deltaTime);

        if (targetCharacterMovement.IsClimbing)
        {
            targetAnimator.speed = Mathf.Abs(inputControl.z);
            targetAnimator.SetFloat(animatorParametersName.ClimbInput, inputControl.z);
        }
        else
        {
            targetAnimator.speed = 1;
            targetAnimator.SetFloat(animatorParametersName.NormalizeMovementX, inputControl.x);
            targetAnimator.SetFloat(animatorParametersName.NormalizeMovementY, inputControl.z);
        }
        
        targetAnimator.SetBool(animatorParametersName.Ground, targetCharacterMovement.IsGrounded);
        targetAnimator.SetBool(animatorParametersName.Climb, targetCharacterMovement.IsClimbing);
        targetAnimator.SetBool(animatorParametersName.Crouch, targetCharacterMovement.IsCrouch);
        targetAnimator.SetBool(animatorParametersName.Sprint, targetCharacterMovement.IsSprint);
        targetAnimator.SetBool(animatorParametersName.Aiming, targetCharacterMovement.IsAiming);

        Vector3 groundSpeed = targetCharacterController.velocity;
        groundSpeed.y = 0;
        targetAnimator.SetFloat(animatorParametersName.GroundSpeed, groundSpeed.magnitude);

        if (!targetCharacterMovement.IsClimbing)
        {
            if (targetCharacterMovement.IsJump)
            {
                if (groundSpeed.magnitude <= 0.01f)
                    CrossFade(jumpIdleFade);
                if (groundSpeed.magnitude > 0.01f)
                    CrossFade(jumpMoveFade);
            }

            if (!targetCharacterMovement.IsGrounded)
            {
                targetAnimator.SetFloat(animatorParametersName.Jump, movementSpeed.y);

                if (movementSpeed.y < 0 && targetCharacterMovement.DistanceToGround > minDistanceToGroundByFall)
                    CrossFade(fallFade);
            }
        }

        targetAnimator.SetFloat(animatorParametersName.DistanceToGround, targetCharacterMovement.DistanceToGround);
    }

    private void CrossFade(AnimationCrossFadeParameters parameters)
    {
        targetAnimator.CrossFade(parameters.name, parameters.duration);
    }
}
