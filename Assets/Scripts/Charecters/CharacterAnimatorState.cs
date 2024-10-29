using System;
using UnityEngine;

public class CharacterAnimatorState : MonoBehaviour
{
    [SerializeField] private CharacterController targetCharacterController;
    [SerializeField] private CharacterMovement targetCharacterMovement;
    [SerializeField] private Animator targetAnimator;
    
    private void LateUpdate()
    {
        Vector3 movementSpeed = transform.InverseTransformDirection(targetCharacterController.velocity); // Change Local to World transform position
        
        targetAnimator.SetFloat("NormalizeMovementX",movementSpeed.x / targetCharacterMovement.GetCurrentSpeed());
        targetAnimator.SetFloat("NormalizeMovementZ",movementSpeed.z / targetCharacterMovement.GetCurrentSpeed());
        
        targetAnimator.SetBool("IsGround", targetCharacterMovement.IsGrounded);
        targetAnimator.SetBool("IsCrouch", targetCharacterMovement.IsCrouch);
        targetAnimator.SetBool("IsSprint", targetCharacterMovement.IsSprint);
        targetAnimator.SetBool("IsAiming", targetCharacterMovement.IsAiming);
        
        if(!targetCharacterMovement.IsGrounded)
            targetAnimator.SetFloat("Jump", movementSpeed.y);
        
        Vector3 groundSpeed = targetCharacterController.velocity;
        groundSpeed.y = 0;
        targetAnimator.SetFloat("GroundSpeed", groundSpeed.magnitude);
        targetAnimator.SetFloat("DistanceToGround", targetCharacterMovement.DistanceToGround);
    }
}
